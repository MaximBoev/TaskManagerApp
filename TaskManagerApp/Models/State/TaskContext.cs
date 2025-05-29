using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public class TaskContext
    {
        public ITaskState State { get; private set; }

        public TaskContext(ITaskState initialState)
        {
            State = initialState;
        }

        public void SetState(ITaskState state)
        {
            State = state;
        }

        public void Next() => State.NextState(this);
        public void Previous() => State.PreviousState(this);
    }
}
