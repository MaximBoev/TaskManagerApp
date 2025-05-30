using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.Prototype
{
    public class TaskPrototypeBuffer
    {
        private static TaskPrototypeBuffer _instance;
        private List<ITaskComponent> _prototypes = new List<ITaskComponent>();

        public static TaskPrototypeBuffer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaskPrototypeBuffer();
                }
                return _instance;
            }
        }

        public void AddPrototype(ITaskComponent task)
        {
            _prototypes.Add(task.Clone());
        }

        public List<ITaskComponent> GetPrototypes()
        {
            return _prototypes.Select(p => p.Clone()).ToList();
        }

        public void Clear()
        {
            _prototypes.Clear();
        }
    }
}
