using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace HackCheck.Data
{
    class CreateAccountMSSQLContext : ICreateAccountContext
    {
        private static readonly string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=HackCheckDB;Integrated Security=False;User Id='HackerCheckMaster'; Password='HackerCheckMasterPassword'";
        private static int saltLengthLimit = 32;

        public bool AddAccountToDatabase(CreateAccountDTO accountDTO)
        {
            accountDTO = DbReadyDTO(accountDTO);

            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [dbo].[Users] SELECT @Username, @Email, @Password, @Salt", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Username", accountDTO.Username));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", accountDTO.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", accountDTO.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Salt", accountDTO.Salt));

                tConnection.Open();

                int tRows = sqlCommand.ExecuteNonQuery();
                if (tRows > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckForEmailTaken(CreateAccountDTO createaccountDTO)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Email = @Email", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Email", createaccountDTO.Email));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        tConnection.Close();
                        return true;
                    }
                    tConnection.Close();
                    return false;
                }
            }
        }

        public bool CheckForUsernameTaken(CreateAccountDTO createaccountDTO)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = @Username", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Username", createaccountDTO.Username));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        tConnection.Close();
                        return true;
                    }
                    tConnection.Close();
                    return false;
                }
            }
        }

        /// <summary>
        /// In this function the data a user fills in is check for a couple things: a server side check if the two passwords match, if the username is already taken, if the email is already taken,
        /// if the user has javascript disabled in his browser or edit's the html of the page this file also checks if the fields are not empty, the email is valid, the username is valid and if 
        /// the password is valid. If any of these conditions return false the user is informed
        /// </summary>
        public bool ValidateAccountCreation(CreateAccountDTO accountDTO)
        {
            // Make sure non of the fields are empty
            if (accountDTO.Username == null || accountDTO.Email == null || accountDTO.Password == null || accountDTO.ConfirmPassword == null)
            {
                return false;
            }

            //Check if the email is actually valid
            try
            {
                var addr = new System.Net.Mail.MailAddress(accountDTO.Email);
            }
            catch
            {
                return false;
            }

            //Check for usename requirments 

            if (accountDTO.Username.Length < 5 || accountDTO.Username.Length > 24)
            {
                return false;
            }

            //Check for password requirments

            if (accountDTO.Password.Length < 6 || accountDTO.Password.Length > 100)
            {
                return false;
            }

            //Check if passwords match

            if (accountDTO.Password != accountDTO.ConfirmPassword)
            {
                return false;
            }

            return true;
        }

        private CreateAccountDTO DbReadyDTO(CreateAccountDTO createaccountDTO)
        {
            CreateAccountDTO DbReadyDTO = new CreateAccountDTO();

            //Getting previous checked data into the model
            DbReadyDTO.Username = createaccountDTO.Username;
            DbReadyDTO.Email = createaccountDTO.Email;
            DbReadyDTO.Salt = GetSalt();

            // Getting position in the alphabet of the first letter in the username and converting it to a index based on ASCII logic
            char firstletter = char.Parse(DbReadyDTO.Username.Substring(0, 1));
            int index = char.ToUpper(firstletter) - 64;

            // If is L or lower puts the salt in front of the password if higher the L puts it after, harder to brute force
            if (index <= 13)
            {
                DbReadyDTO.Password = DbReadyDTO.Salt + createaccountDTO.Password;
            }
            else if (index > 13)
            {
                DbReadyDTO.Password = createaccountDTO.Password + DbReadyDTO.Salt;
            }

            DbReadyDTO.Password = ComputeStringToShHasa256Hash(DbReadyDTO.Password);

            return DbReadyDTO;
        }


        private static string ComputeStringToShHasa256Hash(string plainText)
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
    }
}
