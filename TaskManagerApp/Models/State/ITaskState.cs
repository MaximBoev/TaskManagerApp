using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public interface ITaskState
    {
        string Name { get; }

        void NextState(TaskContext context);
        void PreviousState(TaskContext context);
    }
}
