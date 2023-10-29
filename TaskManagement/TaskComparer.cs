using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TaskManagement
{
    public enum SortTypes
    {
        ByName,
        ByTaskID,
        ByDueDate,
        ByDateAdded
    }

    public class TaskComparer : IComparer<Task>
    {
        SortTypes type;
        public TaskComparer(SortTypes sortTypes)
        {
            type = sortTypes;
        }

        /// <summary>
        /// Compares the Tasks by the SortTypes.
        /// This function is the implementation of the IComparer<T> Compare function.
        /// </summary>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <returns></returns>
        public int Compare(Task task1, Task task2)
        {
            if (type == SortTypes.ByName)
            {
                CultureInfo cultureInfo = new CultureInfo("en-US");

                int stringComparison = string.Compare(task1.Title, task2.Title, cultureInfo,CompareOptions.StringSort);
                Console.WriteLine(stringComparison);
                return stringComparison;
            }            
            if (type == SortTypes.ByTaskID)
            { 
                if (task1.taskID > task2.taskID) return -1;
                return 1;
            }
            if (type == SortTypes.ByDateAdded)
            {
                DateTime[] taskTimeAdded = { task1.DateAdded, task2.DateAdded };

                Array.Sort(taskTimeAdded);

                if (taskTimeAdded[0] == task1.DateAdded) return -1;
                return 1;
            }
            if (type == SortTypes.ByDueDate) //Sorts by date added, in ascending order => 10/23 -> 10/24
            {
                DateTime[] taskDueDate = { task1.DueDate, task2.DueDate };

                Array.Sort(taskDueDate);

                if (taskDueDate[0] == task1.DueDate) return -1;

                return 1;
            }
            return 0;
        }
    }
}
