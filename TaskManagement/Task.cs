using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Specialized;

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
        static int nextTaskID = 0;

        public DateTime DueDate;
        public string Title;
        public Priority PriorityLevel;
        public Status Status;

        public int taskID { get; private set; }
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
        public Task(string title, string dueDate)
        {
            Title = title;
            DueDate = ConvertStringToDateTime(dueDate);

            PriorityLevel = Priority.Medium;
            Status = Status.ToDo;
            DateAdded = DateTime.Now;
            taskID = nextTaskID;


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

    }


}