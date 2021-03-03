using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hack_Check.Models
{
    public class CreateAccountViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
    }
}
