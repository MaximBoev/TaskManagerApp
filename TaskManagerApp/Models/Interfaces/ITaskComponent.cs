using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Memento;

namespace TaskManagerApp.Models
{
    public interface ITaskComponent
    {
        string Name { get; set; }
        TaskStatus Status { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        bool IsExpanded { get; set; }

        // Для CompositeTask
        void AddSubtask(ITaskComponent task);
        void RemoveSubtask(ITaskComponent task);
        //List<ITaskComponent> GetSubtasks();
        IEnumerable<ITaskComponent> Subtasks { get; }

        TaskMemento CreateMemento();
        void RestoreMemento(TaskMemento memento);

        void Tick(DateTime now);
        void Execute(); 

        ITaskComponent Clone();
    }
}
