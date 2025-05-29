using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Memento;

namespace TaskManagerApp.Models
{
    public class TaskCaretaker
    {
        private readonly Stack<TaskMemento> _mementos = new Stack<TaskMemento>();

        public void Save(TaskMemento memento)
        {
            _mementos.Push(memento);
        }

        public TaskMemento Undo()
        {
            return _mementos.Count > 0 ? _mementos.Pop() : null;
        }

        public bool HasUndo => _mementos.Count > 0;
    }
}
