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

namespace reminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<MyDataItem> taskItems = new ObservableCollection<MyDataItem>();
        public MainWindow()
        {
            InitializeComponent();
            taskBox.ItemsSource = taskItems;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddWindow window1 = new AddWindow();
            window1.ShowDialog();
            if (window1.DialogResult == true)
            {
                MyDataItem newItem = new MyDataItem
                {
                    Name = window1.nameBox.Text,
                    Deskription = window1.deskBox.Text,
                    Time = (window1.timeBox.Text),
                    IsChecked = false
                };

                taskItems.Add(newItem);
            }
        }

        private void ListBoxItem_OpenMenu(object sender, MouseButtonEventArgs e)
        {
            this.ContextMenu.Items.Add("Delete");
            this.ContextMenu.IsOpen = true;
        }

        private void TaskComplete(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (taskBox.SelectedItem != null)
            {
                MyDataItem selectedTask = (MyDataItem)taskBox.SelectedItem;
                taskItems.Remove(selectedTask);
            }
        }

        private void taskBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (taskBox.SelectedItem != null)
            {
                MyDataItem selectedTask = (MyDataItem)taskBox.SelectedItem;
                EditWindow editWindow = new EditWindow(selectedTask.Name, selectedTask.Deskription, selectedTask.Time);
                editWindow.ShowDialog();

                if (editWindow.DialogResult == true)
                {
                    selectedTask.Name = editWindow.nameBox.Text;
                    selectedTask.Deskription = editWindow.deskBox.Text;
                    selectedTask.Time = editWindow.timeBox.Text;
                }
            }
        }
    }
}
