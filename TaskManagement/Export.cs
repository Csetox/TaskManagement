using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskManagement
{
    public static class Export
    {
        public static void ExportToJson(User user)
        {
            List<Task> taskList = Database.GetTasksToList(user);

            List<string> JsonStringList = new();

            foreach(Task item in taskList)
            {
                string json = JsonSerializer.Serialize(item);

                JsonStringList.Add(json);
            }

            foreach (var item in JsonStringList)
            {
                Console.WriteLine(item);
            }

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\tasks.json";
            File.WriteAllLines(downloadsPath, JsonStringList);

        }



    }
}
