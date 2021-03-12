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

        public bool ServerSideValidation(CreateAccountViewModel createAccountViewModel) 
        {
            // Make sure non of the fields are empty
            if (createAccountViewModel.Username == null || createAccountViewModel.Email == null || createAccountViewModel.Password == null || createAccountViewModel.ConfirmPassword == null)
            {
                return false;
            }

            //Check if the email is actually valid
            try
            {
                var addr = new System.Net.Mail.MailAddress(createAccountViewModel.Email);
            }
            catch
            {
                return false;
            }

            //Check for usename requirments 

            if (createAccountViewModel.Username.Length < 5 )
            {
                return false;
            }

            if (createAccountViewModel.Username.Length > 24)
            {
                return false;
            }

            //Check for password requirments

            if (createAccountViewModel.Password.Length < 6)
            {
                return false;
            }

            if (createAccountViewModel.Password.Length > 100)
            {
                return false;
            }

            return true;
        }
    }
}
