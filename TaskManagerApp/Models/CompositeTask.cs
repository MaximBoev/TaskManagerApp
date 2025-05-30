using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;
using TaskManagerApp.Models.State;

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

        public TaskContext TaskContext { get; set; } = new TaskContext(new ToDoState());

        public TaskStatus Status
        {
            get
            {
                if (TaskContext.State is ToDoState)
                    return TaskStatus.ToDo;
                else if (TaskContext.State is InProgressState)
                    return TaskStatus.InProgress;
                else if (TaskContext.State is CompletedState)
                    return TaskStatus.Completed;
                else
                    throw new InvalidOperationException("Unknown state");
            }
            set
            {
                // По желанию: вручную изменить состояние
                if (value == TaskStatus.ToDo)
                    TaskContext.SetState(new ToDoState());
                else if (value == TaskStatus.InProgress)
                    TaskContext.SetState(new InProgressState());
                else if (value == TaskStatus.Completed)
                    TaskContext.SetState(new CompletedState());
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

        //public List<ITaskComponent> GetSubtasks() => _subtasks;
        public IEnumerable<ITaskComponent> Subtasks => _subtasks;

        public void Tick(DateTime now)
        {
            foreach (var subtask in _subtasks)
            {
                subtask.Tick(now);
            }

            if (Status == TaskStatus.ToDo && now >= StartTime)
            {
                TaskContext.SetState(new InProgressState());
                OnPropertyChanged(nameof(Status));
                Execute();
            }
            else if (Status == TaskStatus.InProgress && now >= EndTime)
            {
                TaskContext.SetState(new CompletedState());
                OnPropertyChanged(nameof(Status));
                Console.WriteLine($"Составная задача '{Name}' завершена.");
            }
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
            Console.WriteLine($"[CompositeTask] Задача '{Name}' запущена!");
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
