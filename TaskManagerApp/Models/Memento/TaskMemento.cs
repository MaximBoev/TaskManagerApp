using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.Memento
{
    public class TaskMemento
    {
        public string Name { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        //тут мб флаг надо для дерева

        public TaskMemento(string name, DateTime startTime, DateTime endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
