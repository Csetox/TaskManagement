using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{   
    public class TaskManagement
    {
        public void AddTask(User user, Task task)
        {
            user.Tasks.Add(task);
        }
        public void DeleteTask(User user, int taskID)
        {
            Task? taskToRemove = user.Tasks.FirstOrDefault(task => task.taskID == taskID);
            
            if(taskToRemove is not null) user.Tasks.Remove(taskToRemove);
        }
        public void DeleteTask(User user, string taskName)
        {
            var taskToRemove = user.Tasks.Where(task => task.Title == taskName);

            if(taskToRemove is not null)
            {
                if (taskToRemove.ToList().Count > 1) throw new MoreThanOneArgumentException("You have more than one matching tasks!");
                else user.Tasks.Remove(taskToRemove.First());
            }


        }


        public void ListTasks(User user)
        {
            foreach(var item in user.Tasks) Console.WriteLine(item.Title + " " + item.DueDate);
        }



        static void Main()
        {
            User Csetox = new User();
            User Foo = new User();

            TaskManagement Logic = new();

            Logic.AddTask(Csetox, new Task("10/23/2024 16:20:00", "foo", Priority.Medium, Status.Pending));
            Logic.AddTask(Csetox, new Task("10/23 16:20:00", "bar", Priority.Medium, Status.Pending));

            Logic.AddTask(Foo, new Task("10/23/2024 16:20:00", "foo", Priority.Medium, Status.Pending));
            Logic.AddTask(Foo, new Task("10/23/2024 16:20:00", "bar", Priority.Medium, Status.Pending));

            Logic.ListTasks(Csetox);
            Console.WriteLine();


            Logic.DeleteTask(Csetox, "foo");

            Logic.ListTasks(Csetox);

        }

    }
}
