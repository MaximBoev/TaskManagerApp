using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;

namespace TaskManagerApp.Models.Decorator
{
    public class NotificationDecorator : TaskDecorator, ICompositeExtractable
    {
        public NotificationDecorator(ITaskComponent innerTask) : base(innerTask) { }

        public override void Execute()
        {
            _innerTask.Execute();
            Console.WriteLine($" Уведомление: Задача \"{_innerTask.Name}\" скоро начнется в {_innerTask.StartTime}");
        }

        public CompositeTask GetComposite()
        {
            if (_innerTask is ICompositeExtractable extractable)
                return extractable.GetComposite();

            return _innerTask as CompositeTask;
        }

        protected override TaskDecorator CreateNewDecorator(ITaskComponent task)
        {
            return new NotificationDecorator(task);
        }
    }
}
