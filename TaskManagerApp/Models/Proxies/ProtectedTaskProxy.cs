using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;

namespace TaskManagerApp.Models.Proxies
{
    public class ProtectedTaskProxy : ITaskComponent, ICompositeExtractable
    {
        private ITaskComponent _realTask;
        private string _password;

        public ProtectedTaskProxy(ITaskComponent realTask, string password)
        {
            _realTask = realTask;
            _password = password;
        }

        public string Name
        {
            get => _realTask.Name;
            set => _realTask.Name = value;
        }

        public DateTime StartTime
        {
            get => _realTask.StartTime;
            set => _realTask.StartTime = value;
        }

        public DateTime EndTime
        {
            get => _realTask.EndTime;
            set => _realTask.EndTime = value;
        }

        public TaskStatus Status
        {
            get => _realTask.Status;
            set => _realTask.Status = value;
        }
        public bool IsExpanded { get; set; } = false;

        public void AddSubtask(ITaskComponent task) => _realTask.AddSubtask(task);
        public void RemoveSubtask(ITaskComponent task) => _realTask.RemoveSubtask(task);
        //public List<ITaskComponent> GetSubtasks() => _realTask.GetSubtasks();
        public IEnumerable<ITaskComponent> Subtasks => _realTask.Subtasks;
        public void Tick(DateTime now) => _realTask.Tick(now);
        public TaskMemento CreateMemento() => _realTask.CreateMemento();
        public void RestoreMemento(TaskMemento memento) => _realTask.RestoreMemento(memento);

        public void Execute()
        {
            Console.Write("Введите пароль для доступа к задаче: ");
            string input = Console.ReadLine();
            if (input == _password)
            {
                Console.WriteLine("Доступ разрешён.");
                _realTask.Execute();
            }
            else
            {
                Console.WriteLine("Доступ запрещён. Неверный пароль.");
            }
        }

        public ITaskComponent Clone()
        {
            return new ProtectedTaskProxy(_realTask.Clone(), _password);
        }

        public CompositeTask GetComposite()
        {
            if (_realTask is ICompositeExtractable extractable)
                return extractable.GetComposite();

            return _realTask as CompositeTask;
        }
    }
}
