using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Specialized;
using System.Data;
using Org.BouncyCastle.Tls;
using MySql.Data.MySqlClient;

namespace TaskManagement
{

    public enum Priority
    {
        Low,
        Medium,
        High
    }
    public enum Status
    {
        Pending,
        Overdue,
        Completed,
        ToDo
    }
    public class Task
    {
        public int UserID { get; private set; }

        public string Title;
        public DateTime DueDate;
        public Priority PriorityLevel;
        public Status Status;
        public string? Description { get; set; }

        public int taskID { get; private set; }
        static int nextTaskID = 0;

        public DateTime DateAdded { get; private set; }

        public Task(string title, string dueDate, Priority priorityLevel, Status status)
        {   
            Title = title;
            DueDate = ConvertStringToDateTime(dueDate);
            PriorityLevel = priorityLevel;
            Status = status;

            taskID = nextTaskID;
            DateAdded = DateTime.Now;

            nextTaskID++;
        }
        public Task(string title, string dueDate, int userId)
        {
            Title = title;
            DueDate = ConvertStringToDateTime(dueDate);
            UserID = userId;


            PriorityLevel = Priority.Medium;
            Status = Status.ToDo;
            DateAdded = DateTime.Now;
            taskID = nextTaskID;


            nextTaskID++;

            AddTaskToDatabase(title, taskID,DueDate,0);



        }
        public Task(string title, string dueDate, Priority priorityLevel)
        {
            Title = title;
            DueDate = ConvertStringToDateTime(dueDate);
            PriorityLevel = priorityLevel;

            Status = Status.ToDo;
            taskID = nextTaskID;
            DateAdded = DateTime.Now;

            nextTaskID++;
        }
        public Task(string title, string dueDate, Status status)
        {
            Title = title;
            DueDate = ConvertStringToDateTime(dueDate);
            Status = status;

            PriorityLevel = Priority.Medium;
            taskID = nextTaskID;
            DateAdded = DateTime.Now;

            nextTaskID++;
        }

        static DateTime ConvertStringToDateTime(string dateInString)
        { 
            string format1 = "MM/dd/yyyy HH:mm";
            string format2 = "MM/dd HH:mm";

            if(HasYearInDueDate(dateInString)) return DateTime.ParseExact(dateInString, format1,CultureInfo.InvariantCulture);
            return DateTime.ParseExact(dateInString, format2, CultureInfo.InvariantCulture);
        }
        static bool HasYearInDueDate(string dateInString)
        {
            int countOfSlashes = 0;

            for (int i = 0; i < dateInString.Length; i++)
            {
                if (dateInString[i] == '/') countOfSlashes++;
            }
            return countOfSlashes>1;
        }

        private static void AddTaskToDatabase(string title, int taskid,DateTime duedate,int userid)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user=taskUser;database=taskmanagement;password=admin123;"))
            { 
                string query = "INSERT INTO tasks (TaskID,Title,DueDate,DateAdded,UserID) VALUES (@taskid,@title,@duedate,@dateadded,@userid)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taskid", taskid);
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