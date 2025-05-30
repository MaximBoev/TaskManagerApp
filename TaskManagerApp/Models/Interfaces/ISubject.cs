using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.Interfaces
{
    public interface ISubject
    {
        void Attach(ITaskObserver observer);
        void Detach(ITaskObserver observer);
        void Notify();
    }
}
