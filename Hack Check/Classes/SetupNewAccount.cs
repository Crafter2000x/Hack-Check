using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Hack_Check.Models;


namespace Hack_Check.Classes
{
    public class SetupNewAccount
    {
        public CreateAccountViewModel DatabaseReadyCreateAccountViewModel(CreateAccountViewModel FirstPassViewModel) 
        {
            CreateAccountViewModel DatabaseCreateAccountViewModel = new CreateAccountViewModel();

            DatabaseCreateAccountViewModel.Username = FirstPassViewModel.Username;
            DatabaseCreateAccountViewModel.Email = FirstPassViewModel.Email;
            DatabaseCreateAccountViewModel.Salt = BitConverter.ToString(GetSalt());

            // salt not working yet
            return DatabaseCreateAccountViewModel;
        }

        static string ComputeStringToShHasa256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }

        private static int saltLengthLimit = 16;
        private static byte[] GetSalt()
        {
            return GetSalt(saltLengthLimit);
        }
        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

    }
}
