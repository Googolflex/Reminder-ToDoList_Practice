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
using Window = System.Windows.Window;
using System.Drawing;
using Hardcodet.Wpf.TaskbarNotification;

namespace reminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            taskBox.ItemsSource = taskItems;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckTaskTime();
        }

        private void CheckTaskTime()
        {
            foreach (TaskItem taskItem in taskItems)
            {
                if (DateTime.Now >= taskItem.Time && !taskItem.isReminded)
                {
                    taskbarIcon.ShowBalloonTip(taskItem.Name, taskItem.Deskription, BalloonIcon.Info);
                    taskItem.isReminded = true;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddWindow window1 = new AddWindow();
            window1.ShowDialog();
            if (window1.DialogResult == true)
            {
                TaskItem newItem = new TaskItem
                {
                    Name = window1.TaskName,
                    Deskription = window1.TaskDescription,
                    Time = window1.TaskTime,
                    IsChecked = false,
                    IsReminded = false
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
                TaskItem selectedTask = (TaskItem)taskBox.SelectedItem;
                taskItems.Remove(selectedTask);
            }
        }

        private void taskBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (taskBox.SelectedItem != null)
            {
                TaskItem selectedTask = (TaskItem)taskBox.SelectedItem;
                EditWindow editWindow = new EditWindow(selectedTask.Name, selectedTask.Deskription, selectedTask.Time.ToString());
                editWindow.ShowDialog();

                if (editWindow.DialogResult == true)
                {
                    selectedTask.Name = editWindow.EditedName;
                    selectedTask.Deskription = editWindow.EditedDesk;
                    selectedTask.Time = editWindow.EditedDate;
                }
            }
        }
    }
}
