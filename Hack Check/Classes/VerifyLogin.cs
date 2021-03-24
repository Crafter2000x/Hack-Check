using System.Security.Cryptography;
using System.Text;
using Hack_Check.Models;

/// <summary>
/// In this file the login data of the user is checked, first it will check if the data actually is acording to the requirments for that data and then afterwards start comparing the data
/// to what is in the database. To compare the password the salt will also be retrieved 
/// </summary>

namespace Hack_Check.Classes
{
    public class VerifyLogin
    {
        public bool VerifyLoginData(LoginViewModel loginViewModel) 
        {
            Queries queries = new Queries();
            string combined = null;

            if (queries.CheckForUsernameInDatabase(loginViewModel.Username) == false)
            {
                return false;
            }

            string Salt = queries.RetrieveUserSalt(loginViewModel.Username);

            // Getting position in the alphabet of the first letter in the username and converting it to a index based on ASCII logic
            char firstletter = char.Parse(loginViewModel.Username.Substring(0, 1));
            int index = char.ToUpper(firstletter) - 64;

            // If is L or lower puts the salt in front of the password if higher the L puts it after, harder to brute force
            if (index <= 13)
            {
                combined = Salt + loginViewModel.Password;
            }
            else if (index > 13)
            {
                combined = loginViewModel.Password + Salt;
            }

            if (queries.MatchPasswords(ComputeStringToShHasa256Hash(combined)) == false)
            {
                return false;
            }

            return true;
        }
                    
        public bool ServerSideValidation(LoginViewModel loginViewModel) 
        {
            // Make sure non of the fields are empty
            if (loginViewModel.Username == null || loginViewModel.Password == null)
            {
                return false;
            }

            //Check if username is even the allowed lentgh

            if (loginViewModel.Username.Length < 5 || loginViewModel.Username.Length > 24)
            {
                return false;
            }

            //Check if password is even the allowed lentgh

            if (loginViewModel.Password.Length < 6 || loginViewModel.Password.Length > 100)
            {
                return false;
            }

            return true;
        }

        public int FillLoginWithUserId(string Username) 
        {
            Queries queries = new Queries();

            int RetrievedId = queries.RetrieveUserId(Username);

            if ( RetrievedId != -1)
            {
                return RetrievedId;
            }

            return -1;
        }

        public static string ComputeStringToShHasa256Hash(string plainText)
        {
            //Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                //now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }


}
