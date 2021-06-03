using System.Data.SqlClient;
using HackCheck.Data.Classes;

namespace HackCheck.Data
{
    class LoginMSSQLContext : ILoginContext
    {
        private static readonly string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=HackCheckDB;Integrated Security=False;User Id='HackerCheckMaster'; Password='HackerCheckMasterPassword'";

        public int GetUserId(LoginDTO loginDTO)
        {
            int UserId = -1;

            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = @Username", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", loginDTO.Username));
                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserId = (int)reader["Id"];
                            tConnection.Close();
                            return UserId;
                        }
                    }
                    tConnection.Close();
                    return -1;
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

        private bool MatchLoginData(LoginDTO loginDTO)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", loginDTO.Username));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", loginDTO.Password));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //double check
                            if ((string)reader["Password"] == loginDTO.Password)
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

        /// <summary>
        /// In this function the login data of the user is checked, first it will check if the data actually is acording to the requirments for that data and then afterwards start comparing the data
        /// to what is in the database. To compare the password the salt will also be retrieved 
        /// </summary>
        public bool VerifyLoginData(LoginDTO loginDTO)
        {
            string Salt = RetrieveUserSalt(loginDTO.Username);

            if (Salt == null)
            {
                return false;
            }

            // Getting position in the alphabet of the first letter in the username and converting it to a index based on ASCII logic
            char firstletter = char.Parse(loginDTO.Username.Substring(0, 1));
            int index = char.ToUpper(firstletter) - 64;

            // If is L or lower puts the salt in front of the password if higher the L puts it after, harder to brute force
            if (index <= 13)
            {
                loginDTO.Password = Salt + loginDTO.Password;
            }
            else if (index > 13)
            {
                loginDTO.Password = loginDTO.Password + Salt;
            }

            loginDTO.Password = SHA256Encryption.ComputeStringToShHasa256Hash(loginDTO.Password);

            if (MatchLoginData(loginDTO) == false)
            {
                return false;
            }

            return true;
        }

        public bool ValidateLogin(LoginDTO loginDTO)
        {
            // Make sure non of the fields are empty
            if (loginDTO.Username == null || loginDTO.Password == null)
            {
                return false;
            }

            //Check if username is even the allowed lentgh

            if (loginDTO.Username.Length < 5 || loginDTO.Username.Length > 24)
            {
                return false;
            }

            //Check if password is even the allowed lentgh

            if (loginDTO.Password.Length < 6 || loginDTO.Password.Length > 100)
            {
                return false;
            }

            return true;
        }
    }
}
