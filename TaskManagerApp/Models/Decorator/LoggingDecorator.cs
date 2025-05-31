using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;

namespace TaskManagerApp.Models.Decorator
{
    public class LoggingDecorator : TaskDecorator, ICompositeExtractable
    {
        public LoggingDecorator(ITaskComponent innerTask) : base(innerTask) { }

        public override void Execute()
        {
            LogExecution();
            base.Execute();
        }

        private void LogExecution()
        {
            string logPath = "task_log.txt";
            string logEntry = $"[{DateTime.Now}] Выполнение задачи: {Name}, С {StartTime} до {EndTime}";
            File.AppendAllText(logPath, logEntry + Environment.NewLine);
        }

        protected override TaskDecorator CreateNewDecorator(ITaskComponent task)
        {
            return new LoggingDecorator(task);
        }

        public CompositeTask GetComposite()
        {
            return (_innerTask as ICompositeExtractable)?.GetComposite();
        }
    }
}
