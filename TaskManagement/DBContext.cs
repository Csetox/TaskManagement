using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace TaskManagement
{
    class TaskDbContext : DbContext 
    {
        public static readonly string connectionString = "server=localhost;user=taskUser;database=taskmanagement;password=admin123;";
       //public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));
        }
    }
    class Data
    {
        public static void Foo()
        {
            using(TaskDbContext context = new TaskDbContext())
            {
                //context.Tasks.Add(new Task());

                //foobar

            }


        }


    }
}
