using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using TaskManagerApp.Models.Prototype;
using TaskManagerApp.ViewModels;
using System.IO;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ITaskComponent> _rootTasks = new ObservableCollection<ITaskComponent>();
        private readonly Dictionary<ITaskComponent, TaskCaretaker> _caretakers = new Dictionary<ITaskComponent, TaskCaretaker> ();
        private DispatcherTimer _timer;
        private ITaskComponent _selectedTask;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _rootTasks;
            TaskTree.ItemsSource = _rootTasks;
            //LoadDefaultPrototypes();
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            UpdateTaskTree();
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
                TaskHistoryManager.Instance.Save(task);
                TaskStatistics.Instance.IncrementCreated();
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

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask != null)
            {
                SaveMementoForTask(_selectedTask);
                var editWindow = new EditTaskWindow(_selectedTask);
                if (editWindow.ShowDialog() == true)
                {
                    UpdateTaskTree();
                    TaskHistoryManager.Instance.Save(_selectedTask);
                    TaskStatistics.Instance.IncrementCreated();
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования.");
            }
        }

        private void SaveMementoForTask(ITaskComponent task)
        {
            if (!_caretakers.ContainsKey(task))
                _caretakers[task] = new TaskCaretaker();

            _caretakers[task].Save(task.CreateMemento());
        }

        private void UndoTask_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask == null)
            {
                MessageBox.Show("Выберите задачу для отката.");
                return;
            }

            if (_caretakers.TryGetValue(_selectedTask, out var caretaker) && caretaker.HasUndo)
            {
                var memento = caretaker.Undo();
                _selectedTask.RestoreMemento(memento);
                MessageBox.Show("Задача восстановлена.");
                UpdateTaskTree();
            }
            else
            {
                MessageBox.Show("Нет сохранённого состояния.");
            }
        }

        private void SaveAsPrototype_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask != null)
            {
                TaskPrototypeBuffer.Instance.AddPrototype(_selectedTask);
                MessageBox.Show("Задача сохранена в шаблоны.");
            }
            else
            {
                MessageBox.Show("Выберите задачу для сохранения.");
            }
        }

        private void AddFromPrototype_Click(object sender, RoutedEventArgs e)
        {
            var prototypeWindow = new PrototypeWindow();
            if (prototypeWindow.ShowDialog() == true && prototypeWindow.SelectedPrototype != null)
            {
                var task = prototypeWindow.SelectedPrototype;
                _rootTasks.Add(task);
                UpdateTaskTree();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask == null)
            {
                MessageBox.Show("Сначала выберите задачу для удаления.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Вы уверены, что хотите удалить задачу '{_selectedTask.Name}'?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (var task in _rootTasks.ToList())
                {
                    if (task is ICompositeExtractable extractable)
                    {
                        var composite = extractable.GetComposite();
                        if (composite != null && composite.Subtasks.Contains(_selectedTask))
                        {
                            composite.RemoveSubtask(_selectedTask);
                            UpdateTaskTree();
                            _selectedTask = null;
                            return;
                        }
                    }
                }

                if (_rootTasks.Contains(_selectedTask))
                {
                    _rootTasks.Remove(_selectedTask);
                    UpdateTaskTree();
                    _selectedTask = null;
                    return;
                }

                MessageBox.Show("Не удалось удалить задачу. Возможно, это вложенный декоратор или прокси.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDefaultPrototypes()
        {
            var morning = new CompositeTask
            {
                Name = "☀ Утро",
                StartTime = DateTime.Today.AddHours(6),
                EndTime = DateTime.Today.AddHours(9)
            };
            morning.AddSubtask(new SimpleTask { Name = "Проснуться", StartTime = DateTime.Today.AddHours(6), EndTime = DateTime.Today.AddHours(6.5) });
            morning.AddSubtask(new SimpleTask { Name = "Зарядка", StartTime = DateTime.Today.AddHours(6.5), EndTime = DateTime.Today.AddHours(7) });
            morning.AddSubtask(new SimpleTask { Name = "Завтрак", StartTime = DateTime.Today.AddHours(7), EndTime = DateTime.Today.AddHours(7.5) });

            var lunch = new CompositeTask
            {
                Name = "🍽 Обед",
                StartTime = DateTime.Today.AddHours(12),
                EndTime = DateTime.Today.AddHours(14)
            };
            lunch.AddSubtask(new SimpleTask { Name = "Приготовить еду", StartTime = DateTime.Today.AddHours(12), EndTime = DateTime.Today.AddHours(12.5) });
            lunch.AddSubtask(new SimpleTask { Name = "Поесть", StartTime = DateTime.Today.AddHours(12.5), EndTime = DateTime.Today.AddHours(13) });
            lunch.AddSubtask(new SimpleTask { Name = "Немного отдохнуть", StartTime = DateTime.Today.AddHours(13), EndTime = DateTime.Today.AddHours(14) });

            var evening = new CompositeTask
            {
                Name = "🌙 Вечер",
                StartTime = DateTime.Today.AddHours(18),
                EndTime = DateTime.Today.AddHours(22)
            };
            evening.AddSubtask(new SimpleTask { Name = "Ужин", StartTime = DateTime.Today.AddHours(18), EndTime = DateTime.Today.AddHours(18.5) });
            evening.AddSubtask(new SimpleTask { Name = "Свободное время", StartTime = DateTime.Today.AddHours(18.5), EndTime = DateTime.Today.AddHours(20.5) });
            evening.AddSubtask(new SimpleTask { Name = "Подготовка ко сну", StartTime = DateTime.Today.AddHours(20.5), EndTime = DateTime.Today.AddHours(22) });

            TaskPrototypeBuffer.Instance.AddPrototype(morning);
            TaskPrototypeBuffer.Instance.AddPrototype(lunch);
            TaskPrototypeBuffer.Instance.AddPrototype(evening);
        }
        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            string logPath = System.IO.Path.Combine("Logs", "tasks.log");

            if (!File.Exists(logPath))
            {
                MessageBox.Show("Журнал ещё не создан. Выполните хотя бы одну задачу.", "Нет журнала", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = logPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть журнал: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TryExecute_Click(object sender, RoutedEventArgs e)
        {
            _selectedTask?.TryExecute();
        }

        private void TryComplete_Click(object sender, RoutedEventArgs e)
        {
            _selectedTask?.TryComplete();
        }

        private void TryFail_Click(object sender, RoutedEventArgs e)
        {
            _selectedTask?.TryFail();
        }

        private void TrySkip_Click(object sender, RoutedEventArgs e)
        {
            _selectedTask?.TrySkip();
        }
    }
}
