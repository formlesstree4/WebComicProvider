using System.ComponentModel.DataAnnotations;

namespace WebComicProvider.Models.User
{
    public sealed class UserRegisterRequest
    {
        [Required(ErrorMessage = "Your username is certainly not blank! Please type in a non-blank Username", AllowEmptyStrings = false)]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Your password is certainly not blank! Please type in a non-blank Password", AllowEmptyStrings = false)]
        public string Password { get; set; } = "";

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = "";

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Double check what you've typed. The password fields do not match")]
        public string PasswordConfirm { get; set; } = "";
    }
}
