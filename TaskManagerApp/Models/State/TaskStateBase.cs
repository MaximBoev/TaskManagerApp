using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManagerApp.Models.State
{
    public abstract class TaskStateBase
    {
        public virtual void Execute(ITaskComponent task)
        {
            MessageBox.Show("⚠ Задача не может быть выполнена в текущем состоянии.");
        }

        public virtual void Complete(ITaskComponent task)
        {
            MessageBox.Show("⚠ Завершение невозможно в текущем состоянии.");
        }

        public virtual void Fail(ITaskComponent task)
        {
            MessageBox.Show("⚠ Ошибка выполнения недопустима в текущем состоянии.");
        }

        public virtual void Skip(ITaskComponent task)
        {
            MessageBox.Show("⚠ Пропуск недоступен в текущем состоянии.");
        }

        public virtual bool CanEdit(ITaskComponent task) => false;

        public virtual bool CanDelete(ITaskComponent task) => false;
    }
}
