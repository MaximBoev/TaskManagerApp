using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManagerApp.Models.State
{
    public class InProgressState : ITaskState
    {
        public void Enter(ITaskComponent task)
        {
            // опционально: лог или UI-обновление
        }

        public void Tick(ITaskComponent task, DateTime now)
        {
            if (now >= task.EndTime)
            {
                task.TaskContext.TransitionTo(new CompletedState());
            }
        }

        public void Start(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new InProgressState());
        }

        public void Execute(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задача уже выполняется.");
        }

        public void Complete(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new CompletedState());
        }

        public void Fail(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new FailedState());
        }

        public void Skip(ITaskComponent task)
        {
            MessageBox.Show("⛔ Нельзя пропустить задачу, которая уже выполняется.");
        }

        public bool CanEdit(ITaskComponent task) => false;

        public bool CanDelete(ITaskComponent task) => false;
        public string GetStateName() => "ToDo";

        public TaskStatus GetStatus() => TaskStatus.InProgress;
    }
}
