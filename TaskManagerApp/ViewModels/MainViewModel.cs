using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models;

namespace TaskManagerApp.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<ITaskComponent> Tasks { get; set; } = new ObservableCollection<ITaskComponent>();

        public MainViewModel()
        {
            // позже будем сюда добавлять создание задач
        }
    }
}
