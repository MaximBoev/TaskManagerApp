using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models
{
    public class TaskStatistics
    {
        private static TaskStatistics _instance;
        private static readonly object _lock = new object();

        public int CreatedCount { get; private set; } = 0;
        public int ModifiedCount { get; private set; } = 0;
        public int CompletedCount { get; private set; } = 0;
        public int DeletedCount { get; private set; } = 0;

        private TaskStatistics() { }

        public static TaskStatistics Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TaskStatistics();
                        }
                    }
                }
                return _instance;
            }
        }

        public void IncrementCreated() => CreatedCount++;
        public void IncrementModified() => ModifiedCount++;
        public void IncrementCompleted() => CompletedCount++;
        public void IncrementDeleted() => DeletedCount++;

        public void PrintStats()
        {
            Console.WriteLine("\n========== Task Statistics ==========");
            Console.WriteLine($"Created:   {CreatedCount}");
            Console.WriteLine($"Modified:  {ModifiedCount}");
            Console.WriteLine($"Completed: {CompletedCount}");
            Console.WriteLine($"Deleted:   {DeletedCount}");
            Console.WriteLine("====================================\n");
        }
    }
}
