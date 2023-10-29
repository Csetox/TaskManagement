using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{
    public class User
    {
        public string Username;
        private string password;
        public int userID;
        public List<Task> Tasks = new();

    }


}

