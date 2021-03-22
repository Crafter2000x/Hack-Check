using System.ComponentModel.DataAnnotations;

namespace Hack_Check.Models
{
    public class CreateAccountViewModel
    {
        [Required(ErrorMessage ="Please enter a valid username")]
        [MinLength(5, ErrorMessage = "Username needs to be atleast 5 characters long")]
        [MaxLength(24, ErrorMessage = "Username can't be longer then 24 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Please enter a vaild email address")]
        [EmailAddress(ErrorMessage = "Please enter a vaild email address")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter a vaild password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password needs to be atleast 6 characters long")]
        [MaxLength(100, ErrorMessage = "Password can't be longer then 100 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your password to confirm")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your passwords needs to match the confrim password")]
        public string ConfirmPassword { get; set; }

        // Salt gets set during account setup
        public string Salt { get; set; }
    }
}
