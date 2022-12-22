using System.ComponentModel.DataAnnotations;

namespace Restaurant.Services.Identity.Pages.Account.Register
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
        
        public string ReturnUl { get; set; }
        public string RoleName { get; set; } = "User";
    }
}
