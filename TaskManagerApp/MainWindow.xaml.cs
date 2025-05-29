using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TaskManagerApp.Models;
using TaskManagerApp.Models.Interfaces;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ITaskComponent> _rootTasks = new ObservableCollection<ITaskComponent>();
        private DispatcherTimer _timer;
        private ITaskComponent _selectedTask;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _rootTasks;
            TaskTree.ItemsSource = _rootTasks;
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void ShowStats_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow();
            statsWindow.ShowDialog();
        }
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTaskWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true && addWindow.CreatedTask != null)
            {
                var task = addWindow.CreatedTask;
                task.IsExpanded = true;
                _rootTasks.Add(task);
                UpdateTaskTree();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            foreach (var task in _rootTasks)
            {
                task.Tick(now);
            }
        }
        private void AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskTree.SelectedItem is ITaskComponent selected &&
                selected is ICompositeExtractable extractable)
            {
                var composite = extractable.GetComposite();
                if (composite != null)
                {
                    var addWindow = new AddTaskWindow();
                    if (addWindow.ShowDialog() == true)
                    {
                        var subtask = addWindow.CreatedTask;
                        composite.AddSubtask(subtask);

                        TaskHistoryManager.Instance.Save(subtask);
                        TaskStatistics.Instance.IncrementCreated();

                        UpdateTaskTree();
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная задача не является составной.");
                }
            }
        }
        private void TaskTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _selectedTask = e.NewValue as ITaskComponent;
        }
        private void UpdateTaskTree()
        {
            TaskTree.ItemsSource = null;
            TaskTree.ItemsSource = _rootTasks;
        }
    }
}
