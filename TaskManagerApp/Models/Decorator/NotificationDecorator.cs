using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerApp.Models.Interfaces;

namespace TaskManagerApp.Models.Decorator
{
    public class NotificationDecorator : TaskDecorator, ICompositeExtractable
    {
        public NotificationDecorator(ITaskComponent innerTask) : base(innerTask) { }

        public override void Execute()
        {
            base.Execute();
        }

        private bool _notified = false;

        public override void Tick(DateTime now)
        {
            var previousStatus = Status;
            base.Tick(now);

            if (!_notified && previousStatus == TaskStatus.ToDo && Status == TaskStatus.InProgress)
            {
                MessageBox.Show($"🔔 Задача '{Name}' началась!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                _notified = true;
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
            return new NotificationDecorator(task);
        }
    }
}
