using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public class CompletedState : ITaskState
    {
        public string Name => "Completed";

        public void NextState(TaskContext context)
        {
            // Уже самое позднее состояние, ничего не делаем
        }

        public void PreviousState(TaskContext context)
        {
            context.SetState(new InProgressState());
        }
    }
}
