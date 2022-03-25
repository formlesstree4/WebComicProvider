using System.ComponentModel.DataAnnotations;

namespace WebComicProvider.Models.User
{
    public sealed class UserLoginRequest
    {
        [Required(ErrorMessage = "Your username is certainly not blank! Please type in a non-blank Username", AllowEmptyStrings = false)]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Your password is certainly not blank! Please type in a non-blank Password", AllowEmptyStrings = false)]
        public string Password { get; set; } = "";
    }
}
