using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;
using TaskManagerApp.Models.State;

namespace TaskManagerApp.Models
{
    public class CompositeTask : ITaskComponent, ITaskObserver, ICompositeExtractable
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsExpanded { get; set; } = false;

        private List<ITaskComponent> _subtasks = new List<ITaskComponent>();

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

        public void AddSubtask(ITaskComponent task)
        {
            _subtasks.Add(task);
        }

        public void RemoveSubtask(ITaskComponent task)
        {
            _subtasks.Remove(task);
        }

        //public List<ITaskComponent> GetSubtasks() => _subtasks;
        public IEnumerable<ITaskComponent> Subtasks => _subtasks;

        public void Tick(DateTime now)
        {
            foreach (var subtask in _subtasks)
            {
                subtask.Tick(now); // рекурсивно
            }

            if (Status == TaskStatus.ToDo && now >= StartTime)
            {
                TaskContext.SetState(new InProgressState());
                Execute();
            }
            else if (Status == TaskStatus.InProgress && now >= EndTime)
            {
                TaskContext.SetState(new CompletedState());
                Console.WriteLine($"Составная задача '{Name}' завершена.");
            }
        }

        private void NotifySubtasks()
        {
            foreach (var subtask in _subtasks)
            {
                if (subtask is ITaskObserver observer)
                {
                    observer.Update(this.StartTime);
                }
            }
        }

        public void Update(DateTime newTime)
        {
            TimeSpan duration = EndTime - StartTime;
            StartTime = newTime;
            EndTime = StartTime + duration;

            NotifySubtasks(); 
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

            NotifySubtasks();
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
    }
}
