using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace HackCheck.Data
{
    class AccountMSSQLContext : IAccountContext
    {
        private static readonly string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=HackCheckDB;Integrated Security=False;User Id='HackerCheckMaster'; Password='HackerCheckMasterPassword'";

        public AccountDTO RetrieveUserData(int UserId)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM dbo.Users WHERE Id = @Id", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Id", UserId));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AccountDTO accountDTO = new AccountDTO
                            {
                                Id = (int)reader["Id"],
                                Username = (string)reader["Username"],
                                Email = (string)reader["Email"],
                            };

                            tConnection.Close();
                            return accountDTO;
                        }
                    }
                    tConnection.Close();
                    return null;
                }
            }
        }

        public bool CheckUsernameAvailable(AccountDTO accountDTO)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = @Username", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Username", accountDTO.NewUsername));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        tConnection.Close();
                        return false;
                    }
                    tConnection.Close();
                    return true;
                }
            }
        }

        private string RetrieveUserSalt(string Username)
        {
            string UserSalt = null;

            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Salt FROM dbo.Users WHERE Username = @Username", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", Username));
                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserSalt = (string)reader["Salt"];
                            tConnection.Close();
                            return UserSalt;
                        }
                    }

                    tConnection.Close();
                    return null;
                }
            }
        }

        private bool MatchLoginData(AccountDTO accountDTO)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", accountDTO.Username));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", accountDTO.OldPassword));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //double check
                            if ((string)reader["Password"] == accountDTO.OldPassword)
                            {
                                tConnection.Close();
                                return true;
                            }
                        }
                    }
                    tConnection.Close();
                    return false;
                }
            }
        }

        public bool UpdateUsername(AccountDTO accountDTO)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("UPDATE dbo.Users SET Username = @Username WHERE Id = @Id", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", accountDTO.NewUsername));
                sqlCommand.Parameters.Add(new SqlParameter("@Id", accountDTO.Id));


                tConnection.Open();

                int tRows = sqlCommand.ExecuteNonQuery();

                if (tRows > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool UpdatePassword(AccountDTO accountDTO)
        {
            accountDTO = SecurePassword(accountDTO);

            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("UPDATE dbo.Users SET Password = @Password ,Salt = @Salt WHERE Id = @Id", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Password", accountDTO.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Salt", accountDTO.Salt));
                sqlCommand.Parameters.Add(new SqlParameter("@Id", accountDTO.Id));


                tConnection.Open();

                int tRows = sqlCommand.ExecuteNonQuery();

                if (tRows > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ServerSideValidationUsername(AccountDTO accountDTO) 
        {
            // Make sure non of the fields are empty
            if (accountDTO.OldPassword == null || accountDTO.Id <= 0 || accountDTO.Username == null || accountDTO.NewUsername == null)
            {
                return false;
            }

            //Check for username requirments
            if (accountDTO.NewUsername.Length < 5 || accountDTO.NewUsername.Length > 24)
            {
                return false;
            }

            return true;
        }

        public bool ServerSideValidationPassword(AccountDTO accountDTO)
        {
            // Make sure non of the fields are empty
            if (accountDTO.Password == null || accountDTO.ConfirmPassword == null || accountDTO.OldPassword == null || accountDTO.Id <= 0 || accountDTO.Username == null)
            {
                return false;
            }

            //Check if the passwords match
            if (accountDTO.Password != accountDTO.ConfirmPassword)
            {
                return false;
            }

            //Check for password requirments
            if (accountDTO.Password.Length < 6 || accountDTO.Password.Length > 100)
            {
                return false;
            }

            return true;
        }

        

        public bool VerifyLoginData(AccountDTO accountDTO) 
        {
            accountDTO.Salt = RetrieveUserSalt(accountDTO.Username);

            char firstletter = char.Parse(accountDTO.Username.Substring(0, 1));
            int index = char.ToUpper(firstletter) - 64;

            // If is L or lower puts the salt in front of the password if higher the L puts it after, harder to brute force
            if (index <= 13)
            {
                accountDTO.OldPassword = accountDTO.Salt + accountDTO.OldPassword;
            }
            else if (index > 13)
            {
                accountDTO.OldPassword = accountDTO.OldPassword + accountDTO.Salt;
            }

            accountDTO.OldPassword = ComputeStringToShHasa256Hash(accountDTO.OldPassword);

            if (MatchLoginData(accountDTO))
            {
                return true;
            }

            return false;
        }

        private AccountDTO SecurePassword(AccountDTO accountDTO) 
        {
            accountDTO.Salt = GetSalt();

            // Getting position in the alphabet of the first letter in the username and converting it to a index based on ASCII logic
            char firstletter = char.Parse(accountDTO.Username.Substring(0, 1));
            int index = char.ToUpper(firstletter) - 64;

            // If is L or lower puts the salt in front of the password if higher the L puts it after, harder to brute force
            if (index <= 13)
            {
                accountDTO.Password = accountDTO.Salt + accountDTO.Password;
            }
            else if (index > 13)
            {
                accountDTO.Password = accountDTO.Password + accountDTO.Salt;
            }

            accountDTO.Password = ComputeStringToShHasa256Hash(accountDTO.Password);

            return accountDTO;
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

        //Converts the salt into a string to use
        private static readonly int saltLengthLimit = 32;
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
