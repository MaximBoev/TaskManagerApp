using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models.Interfaces
{
    public interface ICompositeExtractable
    {
        CompositeTask GetComposite();
    }
}
