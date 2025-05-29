using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public class InProgressState : ITaskState
    {
        public string Name => "In Progress";

        public void NextState(TaskContext context)
        {
            context.SetState(new CompletedState());
        }

        public void PreviousState(TaskContext context)
        {
            context.SetState(new ToDoState());
        }
    }
}
