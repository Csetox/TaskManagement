using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MySql.Data.MySqlClient;

namespace TaskManagement
{
    public static class Login
    {
        public static User LoginProcess()
        {
            User loggedInUser = null;

            Console.WriteLine("Do you have an account? (y/n)");

            if (!UserInputYesOrNo())
            {
                Console.WriteLine("Do you want to register one? (y/n)");

                if (!UserInputYesOrNo())
                {
                    Console.WriteLine("The app is exiting.");
                    Environment.Exit(0);
                }
                else Register();

            }

            Console.WriteLine("Username: ");

            string usernameAttempt = Console.ReadLine();

            Console.WriteLine("Password: ");

            string passwordAttempt = GetHiddenInput();
            Console.WriteLine();
            if (ValidateUsernameAndPassword(usernameAttempt, passwordAttempt))
            {
                loggedInUser = new User(usernameAttempt, Database.GetUserID(usernameAttempt));
            }

            //Console.WriteLine($"The loggedInUser's username: {loggedInUser.Username}");
            //Console.WriteLine($"The loggedInUser's UserID: {loggedInUser.UserID}");


            return loggedInUser;
        }
        static void Register()
        {
            Console.WriteLine("The username you want to use: ");

            string username = Console.ReadLine();

            Console.WriteLine("The password you want to use: ");

            string password1 = GetHiddenInput();
            Console.WriteLine();

            Console.WriteLine("Repeat the password: ");
            string password2 = GetHiddenInput();

            if (password1 != password2)
            {
                Console.WriteLine("The passwords don't match.\n Do you want to try again? (y/n)");

                if (UserInputYesOrNo()) Register();
                else
                {
                    Console.WriteLine("The app is exiting...");
                    Environment.Exit(0);
                }
            }

            bool foo =Database.RegisterNewUser(username, password1);

        }
        public static bool DoesUserExists(string username)
        {
            List<string> usernames = new();

            using (MySqlConnection connection = new MySqlConnection(Database.ConnectionString))
            {
                connection.Open();
                string query = "SELECT Username FROM Users";

                MySqlCommand command = new MySqlCommand(query, connection);

                var usernamesFromDatabase = command.ExecuteReader();

                if (usernamesFromDatabase.HasRows)
                {
                    while (usernamesFromDatabase.Read())
                        usernames.Add(usernamesFromDatabase.GetString(0));
                }

                usernamesFromDatabase.Close();
                command.Dispose();
            }

            foreach (string item in usernames)
            {
                if (item == username) return true;
            }
            return false;
        }
        private static bool ValidateUsernameAndPassword(string username, string password)
        {
            if (!DoesUserExists(username)) {/* throw new InvalidLoginCredentialsException("Incorrect username and/or password! Try Again!");*/ return false; }

            if (!Database.CheckPasswordForUser(username, password)) {/* throw new InvalidLoginCredentialsException("Incorrect username and/or password! Try Again!");*/return false; }

            return true;
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
