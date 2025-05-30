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
    public class SimpleTask : ITaskComponent, ITaskObserver
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsExpanded { get; set; } = false;

        public void AddSubtask(ITaskComponent task) { /* не используется */ }
        public void RemoveSubtask(ITaskComponent task) { /* не используется */ }
        //public List<ITaskComponent> GetSubtasks() => new List<ITaskComponent>();
        public IEnumerable<ITaskComponent> Subtasks => Enumerable.Empty<ITaskComponent>();

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
                if (value == TaskStatus.ToDo)
                    TaskContext.SetState(new ToDoState()); 
                else if (value == TaskStatus.InProgress)
                    TaskContext.SetState(new InProgressState());
                else if (value == TaskStatus.Completed)
                    TaskContext.SetState(new CompletedState());
            }
        }

        public void Tick(DateTime now)
        {
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
                Console.WriteLine($"Задача '{Name}' завершена.");
            }
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
            Console.WriteLine($"[CompositeTask] Задача '{Name}' запущена!");
        }

        public ITaskComponent Clone()
        {
            return new SimpleTask
            {
                Name = this.Name,
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
    }
}
