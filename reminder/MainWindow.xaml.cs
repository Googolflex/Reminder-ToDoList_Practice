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
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace reminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();
        private DispatcherTimer timer;
        private bool closingFromMenuItem = false;
        private bool isClosingHandled = false;
        string filePath = "tasks.xml";

        public MainWindow()
        {
            this.Closing += MainWindow_Closing;

            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();


            if (File.Exists(filePath))
            {
                taskItems = DeserializeFromXml<ObservableCollection<TaskItem>>("tasks.xml");
                taskBox.ItemsSource = taskItems;
            }
            else
                taskBox.ItemsSource = taskItems;


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
                    taskbarIcon.ShowBalloonTip(taskItem.Name, taskItem.Desсription, BalloonIcon.Info);
                    taskItem.isReminded = true;
                }
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
            if (addWindow.DialogResult == true)
            {
                TaskItem newItem = new TaskItem
                {
                    Name = addWindow.TaskName,
                    Desсription = addWindow.TaskDescription,
                    Time = addWindow.TaskTime,
                    TimeToShow = $"{addWindow.TaskTime.ToShortDateString()} {addWindow.TaskTime.ToShortTimeString()}",
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
            (sender as CheckBox).IsEnabled = false;
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
            if (taskBox.SelectedItem != null && !(taskBox.SelectedItem as TaskItem).IsChecked)
            {
                TaskItem selectedTask = (TaskItem)taskBox.SelectedItem;
                EditWindow editWindow = new EditWindow(selectedTask.Name, selectedTask.Desсription, selectedTask.Time.ToString());
                editWindow.ShowDialog();

                if (editWindow.DialogResult == true)
                {
                    selectedTask.Name = editWindow.EditedName;
                    selectedTask.Desсription = editWindow.EditedDesk;
                    selectedTask.Time = editWindow.EditedDate;
                }
            }
        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            string devName = (sender as MenuItem).Header.ToString();
            if (devName == "Googolflex")
                System.Diagnostics.Process.Start("https://github.com/Googolflex");
            else
                System.Diagnostics.Process.Start("https://github.com/de0f");

        }

        private void taskBar_Click(object sender, RoutedEventArgs e)
        {
            string devName = (sender as MenuItem).Header.ToString();
            if (devName == "Close")
            {
                closingFromMenuItem = true;
                this.Close();
            }
            else if (devName == "Show")
            {
                this.WindowState = WindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (closingFromMenuItem && !isClosingHandled)
            {
                e.Cancel = true;
                MessageBoxResult result = await Task.Run(() =>
                {
                    return MessageBox.Show("Do you want to close ToDoList?", "Reminder", MessageBoxButton.YesNo);
                });
                if (result == MessageBoxResult.No)
                {
                    closingFromMenuItem = false;
                    e.Cancel = true;
                }
                else
                {
                    SerializeToXml("tasks.xml", taskItems);
                    isClosingHandled = true;
                    e.Cancel = false;
                    this.Close();                
                }
            }
            else if (!closingFromMenuItem)
            {
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
                e.Cancel = true;
            }
        }

        private void ClearCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (taskItems != null)
            {
                ObservableCollection<TaskItem> tasksToRemove = new ObservableCollection<TaskItem>();
                foreach (TaskItem item in taskItems)
                {
                    if(item.IsChecked)
                    tasksToRemove.Add(item);
                }
                foreach (TaskItem item in tasksToRemove)
                {
                    taskItems.Remove(item);
                }
            }
        }

        private void taskBox_LostFocus(object sender, RoutedEventArgs e)
        {
            taskBox.SelectedItem = null;
        }
        static void SerializeToXml<T>(string filePath, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }
        static T DeserializeFromXml<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

    }
}
