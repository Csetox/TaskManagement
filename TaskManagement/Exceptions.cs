using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{
    public class MoreThanOneArgumentException : Exception
    {
        public MoreThanOneArgumentException(){}
        public MoreThanOneArgumentException(string message) : base(message){}

    }
    public class InvalidLoginCredentialsException : Exception
    {
        public InvalidLoginCredentialsException(){}

        public InvalidLoginCredentialsException(string mesage) : base(mesage) {}

    }
}
