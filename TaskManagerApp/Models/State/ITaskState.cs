using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public interface ITaskState
    {
        void Tick(ITaskComponent task, DateTime now);
        void Enter(ITaskComponent task); // вызывается при входе в состояние
        void Start(ITaskComponent task);
        void Complete(ITaskComponent task);
        void Fail(ITaskComponent task);
        void Skip(ITaskComponent task);
        string GetStateName();
        TaskStatus GetStatus();
        void Execute(ITaskComponent task);
        bool CanEdit(ITaskComponent task);
        bool CanDelete(ITaskComponent task);
    }
}
