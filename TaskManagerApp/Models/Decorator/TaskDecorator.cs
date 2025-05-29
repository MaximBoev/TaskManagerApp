using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;

namespace TaskManagerApp.Models.Decorator
{
    public abstract class TaskDecorator : ITaskComponent
    {
        protected ITaskComponent _innerTask;

        protected TaskDecorator(ITaskComponent innerTask)
        {
            _innerTask = innerTask;
        }

        public virtual string Name
        {
            get => _innerTask.Name;
            set => _innerTask.Name = value;
        }

        public virtual DateTime StartTime
        {
            get => _innerTask.StartTime;
            set => _innerTask.StartTime = value;
        }

        public virtual DateTime EndTime
        {
            get => _innerTask.EndTime;
            set => _innerTask.EndTime = value;
        }

        public virtual TaskStatus Status
        {
            get => _innerTask.Status;
            set => _innerTask.Status = value;
        }

        public virtual void AddSubtask(ITaskComponent task) => _innerTask.AddSubtask(task);
        public virtual void RemoveSubtask(ITaskComponent task) => _innerTask.RemoveSubtask(task);
        //public virtual List<ITaskComponent> GetSubtasks() => _innerTask.GetSubtasks();
        public IEnumerable<ITaskComponent> Subtasks => _innerTask.Subtasks;
        public virtual void Tick(DateTime now) => _innerTask.Tick(now);
        public virtual TaskMemento CreateMemento() => _innerTask.CreateMemento();
        public bool IsExpanded { get; set; } = false;
        public virtual void RestoreMemento(TaskMemento memento) => _innerTask.RestoreMemento(memento);

        public virtual void Execute() => _innerTask.Execute();

        public virtual ITaskComponent Clone()
        {
            var clonedTask = _innerTask.Clone();
            return CreateNewDecorator(clonedTask);
        }

        protected abstract TaskDecorator CreateNewDecorator(ITaskComponent task);
    }
}
