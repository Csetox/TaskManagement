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
        public void DeleteTask(Task task)
        {

        }
        public void UpdateTask(Task task)
        {

        }

        public void ListTasks(User user)
        {
            foreach(var item in user.Tasks) Console.WriteLine(item.Title);
        }



        static void Main()
        {
            User Csetox = new User();
            Csetox.Username = "Csetox";

            TaskManagement Logic = new();

            Logic.AddTask(Csetox, new Task("October 24, 2023 16:20:00", "foo", Priority.Medium, Status.Pending));

            Logic.ListTasks(Csetox);

        }

    }
}
