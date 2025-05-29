using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public class ToDoState : ITaskState
    {
        public string Name => "To Do";

        public void NextState(TaskContext context)
        {
            context.SetState(new InProgressState());
        }

        public void PreviousState(TaskContext context)
        {
            // Уже самое раннее состояние, ничего не делаем
        }
    }
}
