using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace TaskManagement
{
    public static class TaskManagement
    {
        /// <summary>
        /// Handles the user input of AddTask.
        /// Gets the task's name, due date and other things.
        /// The constructor of Task has the AddTaskToDatabase() function, and it automatically adds it to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="task"></param>
        public static void AddTask(this User user)
        {
            string nameOfTask;
            string dueDateString;
            Console.WriteLine("Name of the task:");

            nameOfTask = Console.ReadLine();


            //If the year is not given it automatically sets the year to the current year even if the date is in the past.
            //TODO: fix this edge case.
            Console.WriteLine("When is it due? (MM/dd/yyyy HH:mm) / (MM/dd HH:mm) / (MM/dd)");

            dueDateString = Console.ReadLine();

            new Task(nameOfTask, dueDateString, user.UserID);
          //  user.Tasks.Add(task);
        }
        
        private static void DeleteTask(this User user)
        {
            int taskID;
            Database.ListTasks(user);

            Console.WriteLine("Which task do you want to delete? Write the TaskID.");
            taskID = Convert.ToInt32(Console.ReadLine());

            Database.DeleteTask(user, taskID);
        }

        /// <summary>
        /// Lists the tasks of the given user.
        /// </summary>
        /// <param name="user"></param>
        public static void ListTasks(this User user)
        {
            Database.ListTasks(user);
        }
        /// <summary>
        /// Sorts the Tasks by their properties using the IComparer<> interface
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        public static void SortTasks(User user, SortTypes type)
        {
            user.Tasks.Sort(new TaskComparer(type));

            foreach (var item in user.Tasks) Console.WriteLine(item.Title);
        }
        private static void GetDescription(this User user)
        {
            int taskID;
            Database.ListTasks(user);
            Console.WriteLine("Which task's description do you want to read? Write the TaskID.");
            taskID = Convert.ToInt32(Console.ReadLine());

            Database.GetDescription(user, taskID);
        }

        public static void AddDescription(this User user)
        {
            int taskID;
            string desc;

            Database.ListTasks(user);

            Console.WriteLine("To which task do you want to add a description? Write the TaskID.");

            taskID = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Write the description.");

            desc = Console.ReadLine();

            Database.AddDescription(user, desc, taskID);
        }

        public static void Run()
        { 
            User user = Login.LoginProcess();
            MainMenu(user);

        }

        private static void MainMenu(User user)
        {
            ConsoleKeyInfo action;
            Console.WriteLine("What do you wanna do? \n1 - Add Task\n2 - Delete Task\n3 - List Task\n4 - Read the description of a task\n5 - Add Description to task\n6 - Exit");

            do
            {
                action = Console.ReadKey(intercept: true);

                if(action.KeyChar != '1' && action.KeyChar != '2' && action.KeyChar != '3' && action.KeyChar != '4' && action.KeyChar != '5' && action.KeyChar != '6') Console.WriteLine("Press one of the 6 keys!");
            } while (action.KeyChar != '1' && action.KeyChar != '2' && action.KeyChar != '3' && action.KeyChar != '4' && action.KeyChar != '5' && action.KeyChar != '6');

            Console.Clear();
            
            switch (action.KeyChar)
            {
                case '1': 
                    user.AddTask(); 
                    break;
                case '2': 
                    user.DeleteTask();
                    break;
                case '3': 
                    user.ListTasks();
                    break;
                case '4':
                    user.GetDescription();
                    break;
                case '5':
                    user.AddDescription(); break;
                case '6':
                    Console.WriteLine("The app is exiting...");
                    Environment.Exit(0);
                    break;
            }

            MainMenu(user);
        }


    }

    
}

    

