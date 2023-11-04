using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskManagement
{
    class Application
    {
        static void Main(string[] args)
        {
            //Console.WriteLine($"The hashed password:{PasswordHashing.Hash("01234")}");
            //Console.WriteLine($"The other hashed password: {PasswordHashing.Hash("01234")}");
            //TaskManagement.Run();

            User user = Login.LoginProcess();

            Export.ExportToJson(user);


            var tasks = new List<Tasked>
        {
            new Tasked { Id = 1, Title = "Complete assignment", IsCompleted = false },
            new Tasked { Id = 2, Title = "Prepare presentation", IsCompleted = true }
        };

            string json = JsonSerializer.Serialize(tasks);

            //Console.WriteLine(json);


        }
    }
    public class Tasked
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
