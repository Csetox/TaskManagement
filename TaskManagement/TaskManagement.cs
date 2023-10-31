﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace TaskManagement
{
    public class TaskManagement
    {
        public void AddTask(User user, Task task)
        {
            user.Tasks.Add(task);
        }

        /// <summary>
        /// Deletes a task identifying by the task's unique taskID
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taskID"></param>
        public void DeleteTask(User user, int taskID)
        {
            Task? taskToRemove = user.Tasks.FirstOrDefault(task => task.taskID == taskID);

            if (taskToRemove is not null) user.Tasks.Remove(taskToRemove);
        }
        /// <summary>
        /// Deletes a task identifying by the task's name. The name may not be unique.
        /// If that happens it throws MoreThanOneArgumentException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taskName"></param>
        /// <exception cref="MoreThanOneArgumentException"></exception>
        public void DeleteTask(User user, string taskName)
        {
            var taskToRemove = user.Tasks.Where(task => task.Title == taskName);

            if (taskToRemove is not null)
            {
                if (taskToRemove.ToList().Count > 1) throw new MoreThanOneArgumentException("You have more than one matching tasks!");
                else user.Tasks.Remove(taskToRemove.First());
            }
        }

        /// <summary>
        /// Lists the tasks of the given user.
        /// </summary>
        /// <param name="user"></param>
        public void ListTasks(User user)
        {
            foreach (var item in user.Tasks) Console.WriteLine(item.Title + " " + item.DueDate);
        }
        /// <summary>
        /// Sorts the Tasks by their properties using the IComparer<!> interface
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        public void SortTasks(User user, SortTypes type)
        {
            user.Tasks.Sort(new TaskComparer(type));

            foreach (var item in user.Tasks) Console.WriteLine(item.Title);
        }

        public void AddDescription(User user, int? taskID, string? taskName, string desc)
        {
            user.Tasks[user.Tasks.FindIndex(task => task.taskID == taskID || task.Title == taskName)].Description = desc;
        }



        public static void Run()
        {

            User user = Login.LoginProcess();

            TaskManagement taskManagement = new();

           

            taskManagement.AddTask(user, new Task("Finish TaskManagement Project", "12/31 23:59", user.UserID));

            Console.WriteLine($"The logged in user's userID: {user.UserID}");


            //Console.WriteLine("Password:\n");
            // string passwd = Console.ReadLine();

            //Login.GetUsername("foo");


            //User Csetox = new User();
            // User Foo = new User();

            //TaskManagement TaskManager = new();

            //TaskManager.AddTask(Csetox, new Task("foo", "10/23/2024 16:20",0));
            //TaskManager.AddTask(Csetox, new Task("bar", "10/23 16:20"));

            // TaskManager.SortTasks(Csetox, SortTypes.ByDueDate);
            // Logic.AddTask(Foo, new Task("10/23/2024 16:20", "foo", Priority.Medium, Status.Pending));
            //Logic.AddTask(Foo, new Task("10/23/2024 16:20", "bar", Priority.Medium, Status.Pending));

            //TaskManager.ListTasks(Csetox);
            //Console.WriteLine();

            //TaskManager.SortTasks(Csetox, SortTypes.ByName);

            //MySqlConnection Connection = new MySqlConnection("server=TaskManagement;user=root;database=taskmanagement;port=3306;password=admin123");

            //DatabaseConnection.DatabaseQuery();


        }


    }

    
}

    

