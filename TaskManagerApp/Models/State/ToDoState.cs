using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Models.State
{
    public class ToDoState : ITaskState 
    {
        public void Enter(ITaskComponent task)
        {
            // опционально: лог или UI-обновление
            var taskName = task?.Name ?? "Неизвестная задача";
            TaskLogger.Log($"[ENTER] Задача '{task.Name}' вошла в состояние {GetStateName()}");
        }

        public void Tick(ITaskComponent task, DateTime now)
        {
            if (now >= task.StartTime)
            {
                task.TaskContext.TransitionTo(new InProgressState());
                task.Execute(); 
            }
        }

        public void Start(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new InProgressState());
        }

        public void Execute(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задачи в статусе 'ToDo' запускаются только автоматически.");
        }

        public void Complete(ITaskComponent task)
        {
            MessageBox.Show("⛔ Нельзя завершить задачу, которая ещё не началась.");
        }

        public void Fail(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задача ещё не выполнялась, нельзя пометить как проваленную.");
        }

        public void Skip(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new SkippedState());
        }

        public bool CanEdit(ITaskComponent task) => true;

        public bool CanDelete(ITaskComponent task) => true;
        public string GetStateName() => "ToDo";

        public TaskStatus GetStatus() => TaskStatus.ToDo;
    }
}
