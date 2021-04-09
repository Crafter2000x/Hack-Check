using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Hack_Check.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hack_Check.Classes
{
    public class Queries
    {
        private static readonly string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=HackCheckDB;Integrated Security=False;User Id='HackerCheckMaster'; Password='HackerCheckMasterPassword'";

        public bool CheckUsernameAlreadyTaken(string Username) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString)) 
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = @Username", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Username",Username));

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

        public bool CheckEmailAlreadyTaken(string Email)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Email = @Email", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Email", Email));

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

        public bool AddAccountToDatabase(CreateAccountViewModel DatabaseReadyViewModel) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [dbo].[Users] SELECT @Username, @Email, @Password, @Salt", tConnection);

                sqlCommand.Parameters.Add(new SqlParameter("@Username", DatabaseReadyViewModel.Username));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", DatabaseReadyViewModel.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", DatabaseReadyViewModel.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Salt", DatabaseReadyViewModel.Salt));

                tConnection.Open();

                int tRows = sqlCommand.ExecuteNonQuery();
                if (tRows > 0)
                {
                    return true;
                }
            }
            return false;
        }  
        
        public bool CheckForUsernameInDatabase(string Username) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Username FROM dbo.Users WHERE Username = @Username", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", Username));
                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if ((string)reader["Username"] == Username)
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

        public string RetrieveUserSalt(string Username) 
        {
            using(SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Salt FROM dbo.Users WHERE Username = @Username", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", Username));
                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        string UserSalt = null;

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

        public int RetrieveUserId(string Username)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = @Username", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", Username));
                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int UserId = -1;

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

        public bool MatchPasswords(string Username, string Password) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", Username));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", Password));

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //double check
                            if ((string)reader["Password"] == Password)
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

        public AccountViewModel AccountInformation(int UserId)
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
                            AccountViewModel accountViewModel = new AccountViewModel
                            {
                                Id = (int)reader["Id"],
                                Username = (string)reader["Username"],
                                Email = (string)reader["Email"],
                            };

                            tConnection.Close();
                            return accountViewModel;
                        }  
                    }
                    tConnection.Close();
                    return null;
                }
            }
        }

        public bool UpdatePasswordInDatabase(AccountViewModel accountViewModel) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("UPDATE dbo.Users SET Password = @Password ,Salt = @Salt WHERE Id = @Id", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Password", accountViewModel.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Salt", accountViewModel.Salt));
                sqlCommand.Parameters.Add(new SqlParameter("@Id", accountViewModel.Id));


                tConnection.Open();

                int tRows = sqlCommand.ExecuteNonQuery();

                if (tRows > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool UpdateUsernameInDatabase(AccountViewModel accountViewModel)
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("UPDATE dbo.Users SET Username = @Username WHERE Id = @Id", tConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", accountViewModel.NewUsername));
                sqlCommand.Parameters.Add(new SqlParameter("@Id", accountViewModel.Id));


                tConnection.Open();

                int tRows = sqlCommand.ExecuteNonQuery();

                if (tRows > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
