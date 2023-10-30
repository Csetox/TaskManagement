using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TaskManagement
{
    public static class Database
    {
        public static string ConnectionString = "server=localhost;user=taskUser;database=taskmanagement;password=admin123;";

        public static void DatabaseQuery()
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;user=taskUser;database=taskmanagement;password=admin123;");

            connection.Open();

            string queryString = "SELECT * FROM Users";

            MySqlCommand command = new MySqlCommand(queryString, connection);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(reader.GetString(i));
                    }
                }
            }

        }
        /// <summary>
        /// Registers the user and the password into the database.
        /// TODO: the password is to be renamed passwordHashed, but I don't implement hashing just yet.
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if the registration was successful.</returns>
        public static bool RegisterNewUser(string username, string password)
        {
            int userID;
            do
            {
                userID = new Random().Next(1, 1000000);

            }while (!IsUserIDTaken(userID));

            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = "INSERT INTO users (UserID,Username,PasswordHash) VALUES (@userid,@username,@password);";

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userid", userID);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();

                    int res = command.ExecuteNonQuery();

                    Console.WriteLine(res);

                    if (res == 1) return true;

                }
            }

            return false;
        }
        private static bool IsUserIDTaken(int userID)
        {
            List<int> allUserIDs = new();

            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = "SELECT UserID FROM Users";

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {

                    connection.Open();

                    var userIDsFromDatabase = command.ExecuteReader();

                    if (userIDsFromDatabase.HasRows)
                    {
                        while (userIDsFromDatabase.Read())
                        {
                            allUserIDs.Add(Convert.ToInt32(userIDsFromDatabase.GetString(0)));
                        } 
                    }
                }
            }

            if (allUserIDs.Contains(userID)) return true;

            return false;
        }



    }
}
