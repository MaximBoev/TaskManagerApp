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

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private ITaskComponent _task;

        public EditTaskWindow(ITaskComponent task)
        {
            InitializeComponent();
            _task = task;
            StartDatePicker.SelectedDate = task.StartTime.Date;
            StartTimeBox.Text = task.StartTime.ToString("HH:mm");
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue && TimeSpan.TryParse(StartTimeBox.Text, out TimeSpan time))
            {
                DateTime newStart = StartDatePicker.SelectedDate.Value.Date + time;
                _task.UpdateTime(newStart);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите корректное время.");
            }
        }
    }

}
