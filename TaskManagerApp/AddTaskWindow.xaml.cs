using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TaskManagerApp.Models;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public ITaskComponent CreatedTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void OnCreateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameBox.Text;
                DateTime date = DateBox.SelectedDate ?? DateTime.Today;

                if (!TimeSpan.TryParse(StartTimeBox.Text, out TimeSpan startTime))
                {
                    MessageBox.Show("Неверный формат времени начала (например: 09:00)");
                    return;
                }

                if (!TimeSpan.TryParse(EndTimeBox.Text, out TimeSpan endTime))
                {
                    MessageBox.Show("Неверный формат времени окончания (например: 10:30)");
                    return;
                }

                var viewModel = new AddTaskViewModel
                {
                    Name = name,
                    StartTime = date.Date + startTime,
                    EndTime = date.Date + endTime,
                    IsComposite = CompositeCheckBox.IsChecked == true,
                    UseDecorator = DecoratorCheckBox.IsChecked == true,
                    UseProtection = ProtectCheckBox.IsChecked == true,
                    Password = PasswordBox.Text,
                    ScriptName = ScriptBox.Text,
                    UseLogging = LoggingCheckBox.IsChecked == true
                };

                CreatedTask = viewModel.CreateTask();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании задачи: {ex.Message}");
            }
        }
    }
}
