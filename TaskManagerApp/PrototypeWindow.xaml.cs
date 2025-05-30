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
using TaskManagerApp.Models.Prototype;
using TaskManagerApp.Models;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for PrototypeWindow.xaml
    /// </summary>
    public partial class PrototypeWindow : Window
    {
        public ITaskComponent SelectedPrototype { get; private set; }

        public PrototypeWindow()
        {
            InitializeComponent();
            LoadPrototypes();
        }

        private void LoadPrototypes()
        {
            PrototypeListBox.ItemsSource = TaskPrototypeBuffer.Instance.GetPrototypes();
        }

        private void UsePrototype_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is ITaskComponent prototype)
            {
                SelectedPrototype = prototype;
                DialogResult = true;
                Close();
            }
        }

        private void ClearPrototypes_Click(object sender, RoutedEventArgs e)
        {
            TaskPrototypeBuffer.Instance.Clear();
            LoadPrototypes();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
