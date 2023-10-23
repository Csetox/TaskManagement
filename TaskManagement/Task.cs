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
        Completed
    }
    public class Task
    {
        static int nextTaskID = 0;

        public DateTime DueDate;
        public string Title;
        public Priority PriorityLevel;
        public Status Status;
        public int taskID { get; private set; }

        public Task(string dueDate, string title, Priority priorityLevel, Status status)
        {
            DueDate = ConvertStringToDateTime(dueDate);
            Title = title;
            PriorityLevel = priorityLevel;
            Status = Status.Pending;
            taskID = nextTaskID;

            nextTaskID++;
        }
        static DateTime ConvertStringToDateTime(string dateInString)
        {


            string format1 = "MM/dd/yyyy HH:mm:ss";
            string format2 = "MM/dd HH:mm:ss";

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