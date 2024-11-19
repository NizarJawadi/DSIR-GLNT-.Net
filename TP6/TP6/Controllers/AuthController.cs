using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP6.Model;
using TP6.Services;

namespace TP6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Constructor for dependency injection
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the AuthService to handle registration
            var result = await _authService.RegisterAsync(model);

            // Handle unsuccessful registration
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            // Return successful result
            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the service to authenticate the user and generate a token
            var result = await _authService.GetTokenAsync(model);

            // If authentication fails, return a bad request with the error message
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            // Return the token and user info if successful
            return Ok(result);
        }


        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the service to add the role to the user
            var result = await _authService.AddRoleAsync(model);

            // If an error message is returned, return a BadRequest with the error message
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            // Return the model as the success response
            return Ok(model);
        }


    }

}
