using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace reminder
{
    public partial class MainWindow : Window
    {
        ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();
        ObservableCollection<String> previousTasks { get; set; } = new ObservableCollection<String>();
        AutoRunManager autoRunManager = new AutoRunManager("ToDoList");
        private DispatcherTimer timer;
        private bool closingFromMenuItem = false;
        private bool isClosingHandled = false;
        string filePath = $"Tasks/{DateTime.Now.ToShortDateString()}.xml";

        public MainWindow()
        {
            this.Closing += MainWindow_Closing;

            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();

            if (!Directory.Exists("Tasks"))
                Directory.CreateDirectory("Tasks");
            else
            {
                string[] strings = new string[Directory.GetFiles("Tasks").Length];
                strings = Directory.GetFiles("Tasks");
                foreach (string file in strings) {
                        string temp = file.Remove(0, 6).Remove(10, 4);
                    if (temp != DateTime.Now.ToShortDateString())
                        previousTasks.Add(temp);
                }
                previousDayMenu.ItemsSource = previousTasks;
            }

            if (File.Exists(filePath))
            {
                taskItems = DeserializeFromXml<ObservableCollection<TaskItem>>($"Tasks/{DateTime.Now.ToShortDateString()}.xml");
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
                if (DateTime.Now >= taskItem.FirstTime && !taskItem.isReminded)
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
            if (addWindow.DialogResult == true && addWindow.IsTimeInterval != true)
            {
                TaskItem newItem = new TaskItem
                {
                    Name = addWindow.TaskName,
                    Desсription = addWindow.TaskDescription,
                    FirstTime = addWindow.TaskTime,
                    TimeToShow = $"{addWindow.TaskTime.ToShortDateString()} {addWindow.TaskTime.ToShortTimeString()}",
                    IsChecked = false,
                    IsReminded = false
                };

                taskItems.Add(newItem);

            }
            else if(addWindow.DialogResult == true && addWindow.IsTimeInterval == true)
            {
                TaskItem newItem = new TaskItem
                {
                    Name = addWindow.TaskName,
                    Desсription = addWindow.TaskDescription,
                    FirstTime = addWindow.TaskTime,
                    SecondTime = addWindow.TaskTime2,
                    TimeToShow = $"{addWindow.TaskTime.ToShortDateString()} {addWindow.TaskTime.ToShortTimeString()} - {addWindow.TaskTime2.ToShortDateString()} {addWindow.TaskTime2.ToShortTimeString()}",
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
                EditWindow editWindow = new EditWindow(selectedTask);
                editWindow.ShowDialog();

                if (editWindow.DialogResult == true)
                {
                    selectedTask = editWindow.editedTask;
                }
            }
        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            string devName = (sender as MenuItem).Header.ToString();
            if (devName == "Googolflex")
                System.Diagnostics.Process.Start("https://github.com/Googolflex");

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
                    SerializeToXml($"Tasks/{DateTime.Now.ToShortDateString()}.xml", taskItems);
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

        private void OpenPreviousDay(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            string day = menuItem.Header.ToString();
            WindowToThePast windowToThePast = new WindowToThePast($"Tasks/{day}.xml");
            windowToThePast.Show();
        }

        private void AutorunOptions_Click(object sender, RoutedEventArgs e)
        {
            MenuItem senderButton = sender as MenuItem;
            if (senderButton.Header.ToString() == "Add to autorun")
            {
                if (!autoRunManager.IsAutoStartEnabled())
                {
                    autoRunManager.AddToAutoStart();
                    MessageBox.Show("The application has been added to autorun");
                }
                else
                    MessageBox.Show("The application has already been added to autorun");
            }
            else
            {
                if (autoRunManager.IsAutoStartEnabled())
                {
                    autoRunManager.RemoveFromAutoStart();
                    MessageBox.Show("The application has been removed from autorun");
                }
                else
                    MessageBox.Show("The application is not in autorun");
            }
        }
    }
}
