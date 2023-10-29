using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TaskManagement
{
    public static class Login
    {
        public static bool CheckIfUserExists(string username)
        {
            List<string> usernames = new();

            using (MySqlConnection connection = new MySqlConnection(DatabaseConnection.ConnectionString))
            {
                connection.Open();
                string query = "SELECT Username FROM Users";

                MySqlCommand command = new MySqlCommand(query, connection);

                var usernamesFromDatabase = command.ExecuteReader();

                if (usernamesFromDatabase.HasRows)
                {
                    while (usernamesFromDatabase.Read())
                    //usernames.Read();
                    usernames.Add(usernamesFromDatabase.GetString(0));
                    
                }

                usernamesFromDatabase.Close();
                command.Dispose();
            }

            foreach(string item in usernames)
            {
                if (item == username) return true;
            }
            return false;
        }
        static string GetHiddenInput()
        {
            string input = "";
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    input += keyInfo.KeyChar;
                    Console.Write("*"); // Display an asterisk for each character
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b"); // Erase the character from the screen
                    }
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);
            return input;
        }

        public static void LoginProcess()
        {
            Console.WriteLine("Username: ");

            string username = Console.ReadLine();
            try
            {
                if (!CheckIfUserExists(username))
                {
                    throw new UserDoesNotExists("This user doesn't exists. Do you want to register a new user? [y/n]");


                }
            }catch(UserDoesNotExists e)
            {
                Console.WriteLine(e.Message);

                char registerOrNot = (char)Console.Read();
                do
                {
                    if (registerOrNot.ToString().ToLower()[0] == 'y') Register();

                    if(registerOrNot.ToString().ToLower()[0] == 'n') Console.WriteLine("The app is exiting...");

                    if (registerOrNot.ToString().ToLower()[0] != 'y' && registerOrNot.ToString().ToLower()[0] != 'n') Console.WriteLine("Press 'y' or 'n'!");

                } while (registerOrNot != 'y' || registerOrNot != 'n');
            }



        }
        static void Register()
        {

        }

    }
}
