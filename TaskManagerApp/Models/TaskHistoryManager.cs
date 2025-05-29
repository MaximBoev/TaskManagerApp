using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models
{
    public class TaskHistoryManager
    {
        private static TaskHistoryManager _instance;
        private static readonly object _lock = new object();

        private Dictionary<ITaskComponent, TaskCaretaker> _taskHistories = new Dictionary<ITaskComponent, TaskCaretaker>();

        private TaskHistoryManager() { }

        public static TaskHistoryManager Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ?? (_instance = new TaskHistoryManager());
                }
            }
        }

        public void Save(ITaskComponent task)
        {
            if (!_taskHistories.ContainsKey(task))
                _taskHistories[task] = new TaskCaretaker();

            var memento = task.CreateMemento();
            _taskHistories[task].Save(memento);
        }

        public void Undo(ITaskComponent task)
        {
            if (_taskHistories.ContainsKey(task) && _taskHistories[task].HasUndo)
            {
                var memento = _taskHistories[task].Undo();
                task.RestoreMemento(memento);
            }
        }

        public bool CanUndo(ITaskComponent task)
        {
            return _taskHistories.ContainsKey(task) && _taskHistories[task].HasUndo;
        }
    }
}
