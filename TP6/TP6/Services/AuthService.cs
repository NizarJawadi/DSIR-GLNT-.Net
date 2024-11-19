using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TP6.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TP6.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }


        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username is already registered!" };
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)

                    errors += $"{error.Description},";
                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };

        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            // Retrieve user-specific claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Retrieve roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            // Combine all claims: default claims + user claims + role claims
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("uid", user.Id) // Custom claim for user ID
    }
            .Union(userClaims) // Add user-specific claims
            .Union(roleClaims); // Add role-specific claims

            // Generate a symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            // Create signing credentials using the key and the HMAC-SHA256 algorithm
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT security token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer, // The issuer of the token
                audience: _jwt.Audience, // The audience for the token
                claims: claims, // All the claims for the user
                expires: DateTime.Now.AddDays(_jwt.DurationInDays), // Expiration date for the token
                signingCredentials: signingCredentials // Signing credentials
            );

            return jwtSecurityToken;
        }


        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Check if the user exists and password is correct
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            // Generate JWT token
            var jwtSecurityToken = await CreateJwtToken(user);

            // Retrieve user roles
            var rolesList = await _userManager.GetRolesAsync(user);

            // Populate AuthModel with user data and token
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }



        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            // Find the user by their ID
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return "Invalid user ID";

            // Check if the role exists
            if (!await _roleManager.RoleExistsAsync(model.Role))
                return "Role does not exist";

            // Check if the user is already in the role
            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User is already assigned to this role";

            // Attempt to assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, model.Role);
            return result.Succeeded ? string.Empty : "Something went wrong while adding the role";
        }


    }
    }

