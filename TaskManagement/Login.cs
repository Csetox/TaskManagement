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
            Console.WriteLine("Do you have an account? (y/n)");

            if (!UserInputYesOrNo())
            {
                Console.WriteLine("Do you want to register one?");

                if (!UserInputYesOrNo())
                {
                    Console.WriteLine("The app is exiting.");
                    Environment.Exit(0);
                }
                else Register();

            }

            Console.WriteLine("Username: ");

            string username = Console.ReadLine();

            Console.WriteLine("Password: ");

            string passwordAttempt = Console.ReadLine();


        }
        
        private static bool UserInputYesOrNo()
        {
            ConsoleKeyInfo consoleKey;
            char userInput;

            do
            {
                consoleKey = Console.ReadKey();
                Erase();

                userInput = char.ToLower(consoleKey.KeyChar);

                if (userInput == 'y') return true;

                if (userInput == 'n') return false;

                if (userInput != 'y' && userInput != 'n') Console.WriteLine("Press 'y' or 'n'!");

            } while (userInput != 'y' && userInput != 'n');
            
            return false;
        }
        static void Register()
        {

        }

        private static void Erase()
        {
            Console.SetCursorPosition(0, Console.CursorTop);

            // Erase the line by overwriting it with spaces
            Console.Write(new string(' ', Console.WindowWidth));

            // Move the cursor back to the beginning
            Console.SetCursorPosition(0, Console.CursorTop);


        }

    }
}
