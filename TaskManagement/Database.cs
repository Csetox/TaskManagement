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

            string queryString = $"SELECT Title,DueDate,TaskID FROM Tasks WHERE UserID = {user.UserID}";

            MySqlCommand command = new MySqlCommand(queryString, connection);

            int titleWidth = 0;
            int dueDateWidth = 0;
            int taskIdWidth = 0;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                // Calculate column widths based on data
                while (reader.Read())
                {
                    string title = reader.GetString(0);
                    DateTime dueDate = reader.GetDateTime(1);
                    int taskID = Convert.ToInt32(reader.GetString(2));

                    if (title.Length > titleWidth)
                        titleWidth = title.Length;

                    string dueDateStr = dueDate.ToString("yyyy-MM-dd"); // Format the date as needed
                    if (dueDateStr.Length > dueDateWidth)
                        dueDateWidth = dueDateStr.Length;

                    string taskIdStr = taskID.ToString();
                    if (taskIdStr.Length > taskIdWidth)
                        taskIdWidth = taskIdStr.Length;

                }

                reader.Close();

                // Print the header
                if (titleWidth < "Title".Length)
                {
                    Console.WriteLine("Title".PadRight(titleWidth) + " | " + "Due Date".PadRight(dueDateWidth) + " | " + "Task ID".PadRight(taskIdWidth));

                    Console.WriteLine(new string('-', titleWidth + dueDateWidth + taskIdWidth + 26));
                }
                else
                {
                    Console.WriteLine("Title".PadRight(titleWidth) + " | " + "Due Date".PadRight(dueDateWidth) + " | " + "Task ID".PadRight(taskIdWidth));

                    Console.WriteLine(new string('-', titleWidth + dueDateWidth + taskIdWidth + 13));
                } 
                    
                }
            // Re-execute the query to get the data
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string title = reader.GetString(0);
                    DateTime dueDate = reader.GetDateTime(1);
                    int taskid = reader.GetInt32(2);
                    string taskID = taskid.ToString();

                    string dueDateStr = dueDate.ToString("yyyy-MM-dd");
                    Console.WriteLine(title.PadRight(titleWidth) + " | " + dueDateStr.PadRight(dueDateWidth) + " | " + taskID.PadRight(taskIdWidth));
                }

            }
            Console.WriteLine();
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
                    command.Parameters.AddWithValue("@password",PasswordHashing.Hash(password));

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
        /// Checks if the given username is already in the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True, if the usernames is already taken, otherwise false.</returns>
        public static bool DoesUserExists(string username)
        {
            List<string> usernames = new();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Username FROM Users";

                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataReader usernamesFromDatabase = command.ExecuteReader();

                if (usernamesFromDatabase.HasRows)
                {
                    while (usernamesFromDatabase.Read())
                        usernames.Add(usernamesFromDatabase.GetString(0));
                }

                usernamesFromDatabase.Close();
                command.Dispose();
            }
            if (usernames.Contains(username)) return true;

            return false;
        }

        /// <summary>
        /// Retrieves the password for the given user, and compares it.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if the passwords match, otherwise false.</returns>
        public static bool CheckPasswordForUser(string username, string passwordAttempt)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT PasswordHash FROM Users WHERE Username=\"{username}\";";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    var passwordHashFromDatabase = command.ExecuteScalar();

                    if (PasswordHashing.Verify(passwordAttempt,passwordHashFromDatabase.ToString())) return true;

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

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.Clear();
                    if(rowsAffected==1) Console.WriteLine("Task successfully added.\n");

                }

            }

        }

        public static void DeleteTask(User user, int taskid)
        {

            List<int> allTaskIDsForUser = GetTaskIDs(user);

            if (!allTaskIDsForUser.Contains(taskid))
            {
                Console.WriteLine("Incorrect TaskID. User doesn't have a task with this TaskID.\n");
                return;
            }

            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM tasks WHERE TaskID = {taskid} AND UserID = {user.UserID};";

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.Clear();
                    if(rowsAffected == 1) Console.WriteLine("Task successfully deleted. \n");
                    else Console.WriteLine("Task deletion unsuccessful.");
                }
            }
        }

        public static void GetDescription(User user, int taskid)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT Description FROM tasks WHERE TaskID = {taskid} AND UserID = {user.UserID};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    var desc = command.ExecuteScalar();

                    Console.WriteLine($"The description:\n{desc}");

                }
                Console.WriteLine();
            }

        }

        public static void AddDescription(User user, string description, int taskid)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"UPDATE tasks SET Description = \"{description}\" WHERE TaskID = {taskid} AND UserID = {user.UserID};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    command.ExecuteNonQuery();

                }
            }

        }
        /// <summary>
        /// Gets all the taskIDs for a given user.
        /// Used for deleting the tasks.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<int> GetTaskIDs(User user)
        {
            List<int> taskIDs = new();

            using (MySqlConnection connection = new MySqlConnection(Database.ConnectionString))
            {
                connection.Open();
                string query = $"SELECT TaskID FROM Tasks WHERE UserID = {user.UserID};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    using (MySqlDataReader taskIdFromDatabase = command.ExecuteReader())
                    {

                        if (taskIdFromDatabase.HasRows)
                        {
                            while (taskIdFromDatabase.Read()) taskIDs.Add(taskIdFromDatabase.GetInt32(0));
                        }
                    }
                }
            }

            return taskIDs;
        }

        public static List<Task> GetTasksToList(User user)
        {
            List<Task> tasks = new();

            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM Tasks WHERE UserID = {user.UserID};";

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int TaskID = reader.GetInt32(0);
                            string Title = reader.GetString(1);
                            string? Description = reader.GetString(2);
                            DateTime DueDate = reader.GetDateTime(3);
                            DateTime AddedDate = reader.GetDateTime(4);
                            string Priority = reader.GetString(5);
                            int UserID = reader.GetInt32(6);
                            bool IsCompleted = reader.GetBoolean(7);

                            tasks.Add(new Task(TaskID, Title, Description, DueDate, AddedDate, Priority, UserID, IsCompleted));
                        }
                    }
                }
            }
            return tasks;
        }

    }
}
