using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.Models.Memento;
using TaskManagerApp.Models.State;

namespace TaskManagerApp.Models.Proxies
{
    public class ProtectedTaskProxy : ITaskComponent, ICompositeExtractable, IScriptAware
    {
        private ITaskComponent _realTask;
        private string _password;

        public string ScriptName
        {
            get
            {
                if (_realTask is IScriptAware s)
                    return s.ScriptName;
                return string.Empty;
            }
            set
            {
                if (_realTask is IScriptAware s)
                    s.ScriptName = value;
            }
        }

        public ProtectedTaskProxy(ITaskComponent realTask, string password)
        {
            _realTask = realTask;
            _password = password;

            _realTask.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, e);
        }

        public string Name
        {
            get => _realTask.Name + " 🔒";
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
            //set => _realTask.Status = value;
        }
        public bool IsExpanded { get; set; } = false;

        public void AddSubtask(ITaskComponent task) => _realTask.AddSubtask(task);
        public void RemoveSubtask(ITaskComponent task) => _realTask.RemoveSubtask(task);
        public IEnumerable<ITaskComponent> Subtasks => _realTask.Subtasks;
        public TaskContext TaskContext => _realTask.TaskContext;

        public void Tick(DateTime now)
        {
            if (_realTask.Status == TaskStatus.ToDo && now >= _realTask.StartTime)
            {
                Execute();
            }
            _realTask.Tick(now);
        }
        public TaskMemento CreateMemento() => _realTask.CreateMemento();
        public void RestoreMemento(TaskMemento memento) => _realTask.RestoreMemento(memento);
        public void UpdateTime(DateTime newStart) => _realTask.UpdateTime(newStart);

        public void Execute()
        {
            if (_realTask.Status == TaskStatus.ToDo)
            {
                var dialog = new PasswordPromptWindow();
                if (dialog.ShowDialog() == true)
                {
                    if (dialog.Password == _password)
                    {
                        MessageBox.Show("✅ Пароль верен. Запуск задачи.", "Доступ разрешён", MessageBoxButton.OK, MessageBoxImage.Information);
                        _realTask.Execute();
                    }
                    else
                    {
                        MessageBox.Show("❌ Неверный пароль. Задача не будет выполнена.", "Доступ запрещён", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                _realTask.Execute();
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TryExecute()
        {
            _realTask.TryExecute();
        }

        public void TryComplete()
        {
            _realTask.TryComplete();
        }

        public void TryFail()
        {
            _realTask.TryFail();
        }

        public void TrySkip()
        {
            _realTask.TrySkip();
        }

        public bool CanEdit()
        {
            return _realTask.CanEdit();
        }

        public bool CanDelete()
        {
            return _realTask.CanDelete();
        }
    }
}
