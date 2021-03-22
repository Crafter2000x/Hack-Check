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
        private string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=HackCheckDB;Integrated Security=False;User Id='HackerCheckMaster'; Password='HackerCheckMasterPassword'";

        public bool CheckUsernameAlreadyTaken(string Username) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString)) 
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = '"+ Username + "'", tConnection);

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
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Email = '" + Email + "'", tConnection);

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
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [dbo].[Users] SELECT '" + DatabaseReadyViewModel.Username + "', '" + DatabaseReadyViewModel.Email + "', '" + DatabaseReadyViewModel.Password + "','" +DatabaseReadyViewModel.Salt+ "';", tConnection);

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
                SqlCommand sqlCommand = new SqlCommand("SELECT Username FROM dbo.Users WHERE Username = '" + Username + "'", tConnection);

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
                SqlCommand sqlCommand = new SqlCommand("SELECT Salt FROM dbo.Users WHERE Username = '" + Username + "'", tConnection);

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
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM dbo.Users WHERE Username = '" + Username + "'", tConnection);

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

        public bool MatchPasswords(string Password) 
        {
            using (SqlConnection tConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Password FROM dbo.Users WHERE Password = '" + Password + "'", tConnection);

                tConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
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
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM dbo.Users WHERE Id = '" + UserId + "'", tConnection);

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
                                Password = (string)reader["Password"],
                                Salt = (string)reader["Salt"]
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
    }
}
