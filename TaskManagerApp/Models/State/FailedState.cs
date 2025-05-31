using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManagerApp.Models.State
{
    public class FailedState : ITaskState
    {
        public void Enter(ITaskComponent task)
        {
            // опционально: лог или UI-обновление
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
            MessageBox.Show("⛔ Повторное выполнение требуется перед завершением.");
        }

        public void Fail(ITaskComponent task)
        {
            MessageBox.Show("⛔ Задача уже отмечена как проваленная.");
        }

        public void Skip(ITaskComponent task)
        {
            task.TaskContext.TransitionTo(new SkippedState());
        }

        public bool CanEdit(ITaskComponent task) => true;

        public bool CanDelete(ITaskComponent task) => true;
        public string GetStateName() => "ToDo";

        public TaskStatus GetStatus() => TaskStatus.Failed;

        public void Tick(ITaskComponent task, DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}
