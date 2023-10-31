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
        /// <summary>
        /// This is the connectionString used for connecting to the local MySql Database.
        /// </summary>
        public static string ConnectionString = "server=localhost;user=taskUser;database=taskmanagement;password=admin123;";
        /// <summary>
        /// Lists the tasks of the given user, in a table.
        /// </summary>
        /// <param name="user"></param>
        public static void ListTasks(User user)
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;user=taskUser;database=taskmanagement;password=admin123;");

            connection.Open();

            string queryString = $"SELECT Title,DueDate FROM Tasks WHERE UserID = {user.UserID}";

            MySqlCommand command = new MySqlCommand(queryString, connection);

            int titleWidth = 0;
            int dueDateWidth = 0;

            using (MySqlDataReader reader = command.ExecuteReader())
            {


                // Calculate column widths based on data
                while (reader.Read())
                {
                    string title = reader.GetString(0);
                    DateTime dueDate = reader.GetDateTime(1);

                    if (title.Length > titleWidth)
                        titleWidth = title.Length;

                    string dueDateStr = dueDate.ToString("yyyy-MM-dd"); // Format the date as needed
                    if (dueDateStr.Length > dueDateWidth)
                        dueDateWidth = dueDateStr.Length;
                }

                reader.Close();

                // Print the header
                Console.WriteLine("Title".PadRight(titleWidth) + " | " + "Due Date".PadRight(dueDateWidth));
                Console.WriteLine(new string('-', titleWidth + dueDateWidth + 3));
            }
            // Re-execute the query to get the data
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string title = reader.GetString(0);
                    DateTime dueDate = reader.GetDateTime(1);

                    string dueDateStr = dueDate.ToString("yyyy-MM-dd");
                    Console.WriteLine(title.PadRight(titleWidth) + " | " + dueDateStr.PadRight(dueDateWidth));
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

            }while (IsUserIDTaken(userID));

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

                    if (res == 1) return true;

                }
            }

            return false;
        }
        /// <summary>
        /// Checks if the given userID is already taken.
        /// Selects all userIDs from the database, and checks if the list of those contain the given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>True, if the userID is taken.</returns>
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
        /// <summary>
        /// Retrieves the password for the given user, and compares it.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if the passwords match, otherwise false.</returns>
        public static bool CheckPasswordForUser(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT PasswordHash FROM Users WHERE Username=\"{username}\";";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    var passwd = command.ExecuteScalar();

                    if (passwd.ToString() == password) return true;

                }
            }
            return false;
        }
        /// <summary>
        /// Gets the unique UserID associated to every user. 
        /// This function only gets called when the user is logged in, so no need to authenticate the user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The UserID of the User that has the username.</returns>
        public static int GetUserID(string username)
        {
            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT UserID FROM Users WHERE Username=\"{username}\";";

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    var userID = command.ExecuteScalar();

                    return (int)userID;
                }
            }
        }
        /// <summary>
        /// This function gets called from the constructor of the Task class. It receives the parameters, and adds it to the database.
        /// TODO: Priority, Status etc. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="taskid"></param>
        /// <param name="duedate"></param>
        /// <param name="userid"></param>
        public static void AddTaskToDatabase(string title, int taskid, DateTime duedate, int userid)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user=taskUser;database=taskmanagement;password=admin123;"))
            {
                string query = "INSERT INTO tasks (Title,DueDate,DateAdded,UserID) VALUES (@title,@duedate,@dateadded,@userid)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    //command.Parameters.AddWithValue("@taskid", taskid);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@duedate", duedate);
                    command.Parameters.AddWithValue("@dateadded", DateTime.Now);
                    command.Parameters.AddWithValue("@userid", userid);

                    connection.Open();

                    int res = command.ExecuteNonQuery();

                    Console.WriteLine(res);

                }

            }

        }
    }
}
