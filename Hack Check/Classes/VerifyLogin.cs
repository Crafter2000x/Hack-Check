using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hack_Check.Models;

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
