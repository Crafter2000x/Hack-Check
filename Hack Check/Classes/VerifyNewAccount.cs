using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hack_Check.Models;

namespace Hack_Check.Classes
{
    public class VerifyNewAccount
    {
        public bool DoThePasswordsMatch(CreateAccountViewModel createAccountViewModel) 
        {
            if (createAccountViewModel.Password == createAccountViewModel.ConfirmPassword)
            {
                return true;
            }
            return false;
        }

        public bool UsernameAlreadyTaken(CreateAccountViewModel createAccountViewModel) 
        {
            Queries queries = new Queries();
            if (queries.CheckUsernameAlreadyTaken(createAccountViewModel.Username) == true)
            {
                return true;
            }
            return false;
        }

        public bool EmailAlreadyTaken(CreateAccountViewModel createAccountViewModel) 
        {
            Queries queries = new Queries();
            if (queries.CheckEmailAlreadyTaken(createAccountViewModel.Email) == true)
            {
                return true;
            }
            return false;
        }
    }
}
