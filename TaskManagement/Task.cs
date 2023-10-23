using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

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
        public DateTime DueDate;
        public string Title;
        public Priority PriorityLevel;
        public Status Status;

        public Task(string dueDate, string title, Priority priorityLevel, Status status)
        {
            DueDate = ConvertStringToDateTime(dueDate);
            Title = title;
            PriorityLevel = priorityLevel;
            Status = Status.Pending; ;
        }
        static DateTime ConvertStringToDateTime(string dateInString)
        {
            string format = "MMMM dd, yyyy HH:mm:ss";

            return DateTime.ParseExact(dateInString, format,CultureInfo.InvariantCulture);

        }

    }


}