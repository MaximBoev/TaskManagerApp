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
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();

            var stats = TaskStatistics.Instance;

            CreatedText.Text = $"Created: {stats.CreatedCount}";
            ModifiedText.Text = $"Modified: {stats.ModifiedCount}";
            CompletedText.Text = $"Completed: {stats.CompletedCount}";
            DeletedText.Text = $"Deleted: {stats.DeletedCount}";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
