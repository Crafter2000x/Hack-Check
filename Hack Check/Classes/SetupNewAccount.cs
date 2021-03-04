using System.Security.Cryptography;
using System.Text;
using Hack_Check.Models;


namespace Hack_Check.Classes
{
    public class SetupNewAccount
    {
        // Making the model ready to be inserted into the database
        public CreateAccountViewModel DatabaseReadyCreateAccountViewModel(CreateAccountViewModel FirstPassViewModel) 
        {
            CreateAccountViewModel DatabaseCreateAccountViewModel = new CreateAccountViewModel();

            //Getting previous checked data into the model
            DatabaseCreateAccountViewModel.Username = FirstPassViewModel.Username;
            DatabaseCreateAccountViewModel.Email = FirstPassViewModel.Email;
            DatabaseCreateAccountViewModel.Salt = GetSalt();

            // Getting position in the alphabet of the first letter in the username and converting it to a index based on ASCII logic
            char firstletter = char.Parse(DatabaseCreateAccountViewModel.Username.Substring(0, 1));
            int index = char.ToUpper(firstletter) - 64;

            // If is L or lower puts the salt in front of the password if higher the L puts it after, harder to brute force
            if ( index <= 13)
            {
                DatabaseCreateAccountViewModel.Password = DatabaseCreateAccountViewModel.Salt + FirstPassViewModel.Password;
            }
            else if (index > 13)
            {
                DatabaseCreateAccountViewModel.Password = FirstPassViewModel.Password + DatabaseCreateAccountViewModel.Salt;
            }

            DatabaseCreateAccountViewModel.Password = ComputeStringToShHasa256Hash(DatabaseCreateAccountViewModel.Password);

            return DatabaseCreateAccountViewModel;
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

        //Converts the salt into a string to use
        private static int saltLengthLimit = 32;
        private static string GetSalt()
        {
            byte[] bytes = GetSalt(saltLengthLimit);
            StringBuilder stringbuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringbuilder.Append(bytes[i].ToString("x2"));
            }

            return stringbuilder.ToString();
        }

        //Generate a salt based on a max lenght and CryptoService
        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public bool AddAccountToDatabase(CreateAccountViewModel DatabaseReadyViewModel) 
        {
            Queries queries = new Queries();

            if (queries.AddAccountToDatabase(DatabaseReadyViewModel) == true)
            {
                return true;
            }
            return false;
        }
    }
}
