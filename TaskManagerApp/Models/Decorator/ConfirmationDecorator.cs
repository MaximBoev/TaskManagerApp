using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models.Interfaces;

namespace TaskManagerApp.Models.Decorator
{
    public class ConfirmationDecorator : TaskDecorator, ICompositeExtractable
    {
        public ConfirmationDecorator(ITaskComponent innerTask) : base(innerTask) { }

        public override void Execute()
        {
            Console.WriteLine($" Подтвердите выполнение задачи \"{_innerTask.Name}\" (y/n):");
            var input = Console.ReadLine();
            if (input?.ToLower() == "y")
            {
                _innerTask.Execute();
                Console.WriteLine(" Подтверждение получено. Задача выполнена.");
            }
            else
            {
                Console.WriteLine(" Выполнение задачи отменено.");
            }
        }

        public CompositeTask GetComposite()
        {
            if (_innerTask is ICompositeExtractable extractable)
                return extractable.GetComposite();

            return _innerTask as CompositeTask;
        }

        protected override TaskDecorator CreateNewDecorator(ITaskComponent task)
        {
            return new ConfirmationDecorator(task);
        }
    }
}
