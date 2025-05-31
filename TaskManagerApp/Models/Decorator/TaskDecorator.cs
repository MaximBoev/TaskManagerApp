using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;
using TaskManagerApp.Models.State;

namespace TaskManagerApp.Models.Decorator
{
    public abstract class TaskDecorator : ITaskComponent, IScriptAware
    {
        protected ITaskComponent _innerTask;

        public virtual string ScriptName
        {
            get => (_innerTask as IScriptAware)?.ScriptName ?? string.Empty;
            set
            {
                if (_innerTask is IScriptAware s)
                    s.ScriptName = value;
            }
        }

        protected TaskDecorator(ITaskComponent innerTask)
        {
            _innerTask = innerTask;
            _innerTask.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, e);
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
            //set => _innerTask.Status = value;
        }

        public virtual void AddSubtask(ITaskComponent task) => _innerTask.AddSubtask(task);
        public virtual void RemoveSubtask(ITaskComponent task) => _innerTask.RemoveSubtask(task);
        public IEnumerable<ITaskComponent> Subtasks => _innerTask.Subtasks;
        public virtual void Tick(DateTime now) => _innerTask.Tick(now);
        public virtual TaskMemento CreateMemento() => _innerTask.CreateMemento();
        public bool IsExpanded { get; set; } = false;
        public virtual void RestoreMemento(TaskMemento memento) => _innerTask.RestoreMemento(memento);
        public void UpdateTime(DateTime newStart) => _innerTask.UpdateTime(newStart);
        public TaskContext TaskContext => _innerTask.TaskContext;
        public virtual void Execute() => _innerTask.Execute();

        public virtual ITaskComponent Clone()
        {
            var clonedTask = _innerTask.Clone();
            return CreateNewDecorator(clonedTask);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract TaskDecorator CreateNewDecorator(ITaskComponent task);

        public void TryExecute()
        {
            _innerTask.TryExecute();
        }

        public void TryComplete()
        {
            _innerTask.TryComplete();
        }

        public void TryFail()
        {
            _innerTask.TryFail();
        }

        public void TrySkip()
        {
            _innerTask.TrySkip();
        }

        public bool CanEdit()
        {
            return _innerTask.CanEdit();
        }

        public bool CanDelete()
        {
            return _innerTask.CanDelete();
        }
    }
}
