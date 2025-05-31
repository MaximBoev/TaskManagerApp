using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Models.State
{
    public class SkippedState : ITaskState
    {
        public void Enter(ITaskComponent task)
        {
            // опционально: лог или UI-обновление
            TaskLogger.Log($"[ENTER] Задача '{task.Name}' вошла в состояние {GetStateName()}");
        }

        public void Start(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new InProgressState());
        }

        public void Execute(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new ToDoState());
        }

        public void Complete(ITaskComponent task)
        {
            MessageBox.Show("⛔ Сначала повторно активируйте задачу.");
        }

        public void Fail(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new FailedState());
        }

        public void Skip(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задача уже пропущена.");
        }

        public bool CanEdit(ITaskComponent task) => true;

        public bool CanDelete(ITaskComponent task) => true;
        public string GetStateName() => "Skipped";

        public TaskStatus GetStatus() => TaskStatus.Skipped;

        public void Tick(ITaskComponent task, DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}
