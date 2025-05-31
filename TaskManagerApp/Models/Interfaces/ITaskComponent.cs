using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Memento;
using TaskManagerApp.Models.State;

namespace TaskManagerApp.Models
{
    public interface ITaskComponent : INotifyPropertyChanged
    {
        string Name { get; set; }
        TaskStatus Status { get; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        bool IsExpanded { get; set; }
        TaskContext TaskContext { get; }

        // Для CompositeTask
        void AddSubtask(ITaskComponent task);
        void RemoveSubtask(ITaskComponent task);
        //List<ITaskComponent> GetSubtasks();
        IEnumerable<ITaskComponent> Subtasks { get; }

        TaskMemento CreateMemento();
        void RestoreMemento(TaskMemento memento);

        void Tick(DateTime now);
        void Execute();

        void TryExecute();
        void TryComplete();
        void TryFail();
        void TrySkip();
        bool CanEdit();
        bool CanDelete();

        ITaskComponent Clone();

        void UpdateTime(DateTime newStart);
    }
}
