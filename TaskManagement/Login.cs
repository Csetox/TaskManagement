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
        /// <summary>
        /// Handles the full login process. 
        /// Checks if the users has an account, do the user want to make an account, and if the user has an account, logs them in.
        /// </summary>
        /// <returns>The object of the logged in user.</returns>
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

            Authenticate(out loggedInUser);
            Console.Clear();

            return loggedInUser;
        }
        /// <summary>
        /// Handles the authenticating process. 
        /// If an invalid username or password is given, the InvalidLoginCredentialsException is thrown 
        /// and the user can decide if they want to try logging in again.
        /// </summary>
        /// <param name="loggedInUser"></param>
        private static void Authenticate(out User loggedInUser)
        {
            loggedInUser = null;
            string usernameAttempt;
            string passwordAttempt;

            Console.WriteLine("Username: ");
            usernameAttempt = Console.ReadLine();

            Console.WriteLine("Password: ");
            passwordAttempt = GetHiddenInput();

            Console.WriteLine();
            
            try
            {
                if (ValidateUsernameAndPassword(usernameAttempt, passwordAttempt))
                {
                    loggedInUser = new User(usernameAttempt, Database.GetUserID(usernameAttempt));
                }
            }
            catch (InvalidLoginCredentialsException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Do you want to try again? (y/n)");

                if (UserInputYesOrNo()) Authenticate(out loggedInUser);
                else
                {
                    Console.WriteLine("The app is exiting...");
                    Environment.Exit(0);
                }
            }

        }


        /// <summary>
        /// Handles the registering part. Checks if the username is taken, or the passwords match.
        /// If everything is good, it registers the account via the Database.RegisterNewUser() function.
        /// </summary>
        static void Register()
        {
            Console.WriteLine("The username you want to use: ");

            string username = Console.ReadLine();

            Console.WriteLine("The password you want to use: ");

            string password1 = GetHiddenInput();
            Console.WriteLine();

            Console.WriteLine("Repeat the password: ");
            string password2 = GetHiddenInput();

            if (DoesUserExists(username))
            {
                Console.WriteLine("Username already taken.\nDo you want to try again? (y/n)");

                if (UserInputYesOrNo()) Register();
                else
                {
                    Console.WriteLine("The app is exiting...");
                    Environment.Exit(0);
                }
            }

            if (password1 != password2)
            {
                Console.WriteLine("The passwords don't match.\nDo you want to try again? (y/n)");

                if (UserInputYesOrNo()) Register();
                else
                {
                    Console.WriteLine("The app is exiting...");
                    Environment.Exit(0);
                }
            }

            //It's true if the registration was successful.
            bool foo =Database.RegisterNewUser(username, password1);

        }
        /// <summary>
        /// Checks if the given username is already in the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True, if the usernames is already taken, otherwise false.</returns>
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

            if (usernames.Contains(username)) return true;

            return false;
        }

        /// <summary>
        /// Validates the username and password combo. Checks if the username exists in the database, and checks if the password is the one used by that username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>False if the username doesn't exists in the database, or the password is incorrect. Otherwise true.</returns>
        private static bool ValidateUsernameAndPassword(string username, string password)
        {
            if (!DoesUserExists(username)) { throw new InvalidLoginCredentialsException("Incorrect username and/or password! Try Again!"); return false; }

            if (!Database.CheckPasswordForUser(username, password)) { throw new InvalidLoginCredentialsException("Incorrect username and/or password! Try Again!"); return false; }

            return true;
        }
        /// <summary>
        /// This function is used when the user is making a decision (y/n).
        /// Once the key is pressed, no need to press enter.
        /// Doesn't accept any other key than 'y' and 'n' and the uppercase parts of them.
        /// </summary>
        /// <returns>True if the user pressed 'y'. False if the user pressed 'n'.</returns>
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
        /// <summary>
        /// Erases the input and certain lines.
        /// </summary>
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
