using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Decorator;
using TaskManagerApp.Models.Proxies;
using TaskManagerApp.Models;
using TaskManagerApp.Models.Memento;

namespace TaskManagerApp.ViewModels
{
    public class AddTaskViewModel
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddMinutes(30);

        public bool IsComposite { get; set; }
        public bool UseProtection { get; set; }
        public string Password { get; set; }
        public bool UseDecorator { get; set; }

        public ITaskComponent CreateTask()
        {
            ITaskComponent task;

            if (IsComposite)
                task = new CompositeTask { Name = Name, StartTime = StartTime, EndTime = EndTime };
            else
                task = new SimpleTask { Name = Name, StartTime = StartTime, EndTime = EndTime };

            if (UseDecorator)
                task = new NotificationDecorator(task);

            if (UseProtection && !string.IsNullOrWhiteSpace(Password))
                task = new ProtectedTaskProxy(task, Password);

            TaskHistoryManager.Instance.Save(task);

            TaskStatistics.Instance.IncrementCreated();

            return task;
        }
    }
}
