using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.State
{
    public class TaskContext
    {
        private ITaskState _state;

        public TaskContext(ITaskState initialState)
        {
            TransitionTo(initialState);
        }

        public void TransitionTo(ITaskState newState)
        {
            _state = newState;
            _state.Enter(Owner);
        }

        public ITaskComponent Owner { get; set; }
        public void Tick(ITaskComponent task, DateTime now) => _state.Tick(task, now);
        public void Start() => _state.Start(Owner);
        public void Complete() => _state.Complete(Owner);
        public void Fail() => _state.Fail(Owner);
        public void Skip() => _state.Skip(Owner);
        public string GetStateName() => _state.GetStateName();
        public TaskStatus GetStatus() => _state.GetStatus();
        public void TryExecute() => _state.Execute(Owner);
        public void TryComplete() => _state.Complete(Owner);
        public void TryFail() => _state.Fail(Owner);
        public void TrySkip() => _state.Skip(Owner);
        public bool CanEdit() => _state.CanEdit(Owner);
        public bool CanDelete() => _state.CanDelete(Owner);
    }
}
