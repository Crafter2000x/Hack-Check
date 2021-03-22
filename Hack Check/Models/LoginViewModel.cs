using System.ComponentModel.DataAnnotations;

namespace Hack_Check.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
