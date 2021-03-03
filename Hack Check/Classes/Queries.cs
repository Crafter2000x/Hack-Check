using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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





    }
}
