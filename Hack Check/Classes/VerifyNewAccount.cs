using Hack_Check.Models;

/// <summary>
/// In this file the data a user fills in is check for a couple things: a server side check if the two passwords match, if the username is already taken, if the email is already taken,
/// if the user has javascript disabled in his browser or edit's the html of the page this file also checks if the fields are not empty, the email is valid, the username is valid and if 
/// the password is valid. If any of these conditions return false the user is informed
/// </summary>

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

            if (createAccountViewModel.Username.Length < 5 || createAccountViewModel.Username.Length > 24)
            {
                return false;
            }

            //Check for password requirments

            if (createAccountViewModel.Password.Length < 6 || createAccountViewModel.Password.Length > 100)
            {
                return false;
            }

            return true;
        }
    }
}
