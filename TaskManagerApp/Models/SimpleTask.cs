using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;
using TaskManagerApp.Models.State;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Models
{
    public class SimpleTask : ITaskComponent, ITaskObserver, IScriptAware
    {
        public string Name { get; set; }
        public string ScriptName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsExpanded { get; set; } = false;

        public void AddSubtask(ITaskComponent task) { }
        public void RemoveSubtask(ITaskComponent task) { }
        public IEnumerable<ITaskComponent> Subtasks => Enumerable.Empty<ITaskComponent>();

        public TaskContext TaskContext { get; private set; }

        private TaskStatus _status;
        public TaskStatus Status
        {
            get => _status;
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public SimpleTask()
        {
            TaskContext = new TaskContext(new ToDoState())
            {
                Owner = this
            };
            Status = TaskContext.GetStatus();
        }

        public void Tick(DateTime now)
        {

            if (Status == TaskStatus.ToDo && now >= StartTime)
                TaskContext.Start();
            else if (Status == TaskStatus.InProgress && now >= EndTime)
                TaskContext.Complete();

            Status = TaskContext.GetStatus();

            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
        }

        public void Update(DateTime newTime)
        {
            TimeSpan duration = EndTime - StartTime;
            StartTime = newTime;
            EndTime = StartTime + duration;
            OnPropertyChanged(nameof(StartTime));
        }

        public TaskMemento CreateMemento()
        {
            return new TaskMemento(Name, StartTime, EndTime);
        }

        public void RestoreMemento(TaskMemento memento)
        {
            Name = memento.Name;
            StartTime = memento.StartTime;
            EndTime = memento.EndTime;
        }

        public void Execute()
        {
            TaskLogger.Log($"Выполнена простая задача: '{Name}' | Время: {StartTime:t} - {EndTime:t}");
        }

        public ITaskComponent Clone()
        {
            return new SimpleTask
            {
                Name = this.Name,
                ScriptName = this.ScriptName,
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateTime(DateTime newStartTime)
        {
            TimeSpan duration = EndTime - StartTime;
            StartTime = newStartTime;
            EndTime = StartTime + duration;

            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
        }
        public void TryExecute() 
        {
            TaskContext.TryExecute();
            OnPropertyChanged(nameof(Status));
        }

        public void TryComplete()
        {
            TaskContext.TryComplete();
            OnPropertyChanged(nameof(Status));
        }
        public void TryFail()
        {
            TaskContext.TryFail();
            OnPropertyChanged(nameof(Status));
        }
        public void TrySkip()
        {
            TaskContext.TrySkip();
            OnPropertyChanged(nameof(Status));
        }

        public bool CanEdit()
        {
            return TaskContext.CanEdit();
        }
        public bool CanDelete()
        {
            return TaskContext.CanDelete();
        }
    }
}
