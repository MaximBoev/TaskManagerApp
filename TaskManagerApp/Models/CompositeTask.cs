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
    public class CompositeTask : ITaskComponent, ITaskObserver, ICompositeExtractable, ISubject
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsExpanded { get; set; } = false;

        private List<ITaskComponent> _subtasks = new List<ITaskComponent>();

        private readonly List<ITaskObserver> _observers = new List<ITaskObserver>();

        public TaskContext TaskContext { get; private set; }

        public CompositeTask()
        {
            TaskContext = new TaskContext(new ToDoState())
            {
                Owner = this
            };
            Status = TaskContext.GetStatus();
        }
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

        public void Attach(ITaskObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(ITaskObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this.StartTime);
            }
            OnPropertyChanged(nameof(StartTime));
        }

        public void AddSubtask(ITaskComponent task)
        {
            _subtasks.Add(task);
            if (task is ITaskObserver observer)
            {
                Attach(observer);
            }
        }

        public void RemoveSubtask(ITaskComponent task)
        {
            _subtasks.Remove(task);
            if (task is ITaskObserver observer)
            {
                Detach(observer);
            }
        }

        public IEnumerable<ITaskComponent> Subtasks => _subtasks;

        public void Tick(DateTime now)
        {
            foreach (var subtask in _subtasks)
            {
                subtask.Tick(now);
            }

            TaskContext.Tick(this, now);
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

            Notify(); 
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

            Notify();
        }

        public void Execute()
        {
            TaskLogger.Log($"Выполнена составная задача: '{Name}' | Время: {StartTime:t} - {EndTime:t}");
        }


        public void TryExecute()
        {
            foreach (var subtask in _subtasks)
            {
                subtask.TryExecute();
                OnPropertyChanged(nameof(Status));
            }
        }

        public void TryComplete()
        {
            foreach (var subtask in _subtasks)
            {
                subtask.TryComplete();
                OnPropertyChanged(nameof(Status));
            }
        }

        public void TryFail()
        {
            foreach (var subtask in _subtasks)
            {
                subtask.TryFail();
                OnPropertyChanged(nameof(Status));
            }
        }

        public void TrySkip()
        {
            foreach (var subtask in _subtasks)
            {
                subtask.TrySkip();
                OnPropertyChanged(nameof(Status));
            }
        }

        public bool CanEdit()
        {
            return _subtasks.All(t => t.CanEdit());
        }

        public bool CanDelete()
        {
            return _subtasks.All(t => t.CanDelete());
        }

        public ITaskComponent Clone()
        {
            var clone = new CompositeTask
            {
                Name = this.Name,
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };

            foreach (var subtask in _subtasks)
            {
                clone.AddSubtask(subtask.Clone());
            }

            return clone;
        }

        public CompositeTask GetComposite()
        {
            return this;
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

            Notify();
        }
    }
}
