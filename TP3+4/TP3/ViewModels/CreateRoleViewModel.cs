using System.ComponentModel.DataAnnotations;

namespace TP3.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]

        public string RoleName { get; set; }
    }
}
