using System;
using System.Linq;
using System.Collections.Generic;

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
    class Task
    {
        public DateTime DueDate;
        public string Title;
        public Priority PriorityLevel;
        public Status Status;

        public Task(DateTime dueDate, string title, Priority priorityLevel, Status status)
        {
            DueDate = dueDate;
            Title = title;
            PriorityLevel = priorityLevel;
            Status = status;
        }

        public void AddTask()
        {

        }

        public void 

    }


}