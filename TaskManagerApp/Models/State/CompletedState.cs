using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Models.State
{
    public class CompletedState : ITaskState
    {
        public void Enter(ITaskComponent task)
        {
            // опционально: лог или UI-обновление
            TaskLogger.Log($"[ENTER] Задача '{task.Name}' вошла в состояние {GetStateName()}");
        }

        public void Execute(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задача уже завершена.");
        }

        public void Complete(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задача уже завершена.");
        }

        public void Fail(ITaskComponent task)
        {
            MessageBox.Show("⛔ Завершённую задачу нельзя провалить.");
        }

        public void Skip(ITaskComponent task)
        {
            MessageBox.Show("⛔ Завершённую задачу нельзя пропустить.");
        }

        public bool CanEdit(ITaskComponent task) => false;

        public bool CanDelete(ITaskComponent task) => false;
        public string GetStateName() => "Completed";

        public TaskStatus GetStatus() => TaskStatus.Completed;

        public void Tick(ITaskComponent task, DateTime now)
        {
            throw new NotImplementedException();
        }

        public void Start(ITaskComponent task)
        {
            throw new NotImplementedException();
        }
    }
}
