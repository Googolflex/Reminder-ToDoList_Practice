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

namespace reminder
{
    public partial class MainWindow : Window
    {
        //Creating collection for storing tasks based on task item model
        ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();

        //Creating class instance for work with autorun
        AutoRunManager autoRunManager = new AutoRunManager("ToDoList");

        //Creating class instance for work with xml
        XmlManager xmlManager = new XmlManager();

        //Creating class instance for work with tasks(edit, add new)
        TasksManager tasksManager = new TasksManager();

        //Paths to autorun and tasks save file
        Path path = new Path();

        //Timer
        private DispatcherTimer timer;

        /*Parameters for prevent app closing from window
            When closing invoked from taskbar first parameter sets to true*/
        private bool closingFromMenuItem = false;
        private bool isClosingHandled = false;

        public MainWindow()
        {
            this.Closing += MainWindow_Closing;

            InitializeComponent();

            //Timer parameters
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();

            /*If file directory exists and save file in it exists to
                        then load tasks to collection from it
                                    else create directory*/
            if (!Directory.Exists("Tasks"))
                Directory.CreateDirectory("Tasks");
            else
            {
                //previousDayMenu.ItemsSource = tasksManager.previousDaysTasks();
            }

            if (File.Exists(path.FilePath))
            {
                taskItems = tasksManager.lodadTasksFromXml();
                taskBox.ItemsSource = taskItems;
            }
            else
                taskBox.ItemsSource = taskItems;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //At each tick, checks whether any notifications need to be sent
            CheckTaskTime();
        }

        private void CheckTaskTime()
        {
            //If time in task < time now then send an notification
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
            //Open window for add an task. Needs to be reworked
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
            if (addWindow.DialogResult == true) { taskItems.Add(addWindow.newItem); }
        }

        private void ListBoxItem_OpenMenu(object sender, MouseButtonEventArgs e)
        {
            //Opens tasks context menu
            this.ContextMenu.Items.Add("Delete");
            this.ContextMenu.IsOpen = true;
        }

        private void TaskComplete(object sender, RoutedEventArgs e)
        {
            //Disable checked checkbox (checked checkbox :) )
            (sender as CheckBox).IsEnabled = false;
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            //Deletes selected task when delete button is pressed 
            if (taskBox.SelectedItem != null)
            {
                TaskItem selectedTask = (TaskItem)taskBox.SelectedItem;
                taskItems.Remove(selectedTask);
            }
        }

        private void EditTask(object sender, MouseButtonEventArgs e)
        {
            //Opens edit window for selected task when edit button is pressed and selected task is active. Needs to be reworked
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
            //Opens my github. It made this way cause earlier it was project for college practice and I made it not alone
            string devName = (sender as MenuItem).Header.ToString();
            System.Diagnostics.Process.Start($"https://github.com/{devName}");

        }

        private void taskBar_Click(object sender, RoutedEventArgs e)
        {
            //Defines what option is clicked in taskbar
            string option = (sender as MenuItem).Header.ToString();
            if (option == "Close")
            {
                //Invokes closing from taskbar
                closingFromMenuItem = true;
                this.Close();
            }
            else if (option == "Show")
            {
                //Causes the window to be shown
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
                    xmlManager.SerializeToXml($"Tasks/{DateTime.Now.ToShortDateString()}.xml", taskItems);
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
            //Clears all completed tasks
            if (taskItems != null)
            {
                foreach (TaskItem item in tasksManager.tasksToRemoveCollection(taskItems))
                {
                    taskItems.Remove(item);
                }
            }
        }

        private void taskBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //When user clicks away from selected task its lost focus
            taskBox.SelectedItem = null;
        }

        private void OpenPreviousDay(object sender, RoutedEventArgs e)
        {
            //Opens window with old task (for selected day). LEGACY! NEEDS TO BE DELETED!! 
            MenuItem menuItem = sender as MenuItem;
            string day = menuItem.Header.ToString();
            WindowToThePast windowToThePast = new WindowToThePast($"Tasks/{day}.xml");
            windowToThePast.Show();
        }

        private void AutorunOptions_Click(object sender, RoutedEventArgs e)
        {
            //Defines selected autorun option and invokes it
            MenuItem senderButton = sender as MenuItem;
            if (senderButton.Header.ToString() == "Add to autorun")
            {
                //If autorun is already enabled drops message
                if (!autoRunManager.IsAutoRunEnabled())
                {
                    autoRunManager.AddToAutoRun();
                    MessageBox.Show("The application has been added to autorun");
                }
                else
                    MessageBox.Show("The application has already been added to autorun");
            }
            else
            {
                //If autorun is already disabled drops message
                if (autoRunManager.IsAutoRunEnabled())
                {
                    autoRunManager.RemoveFromAutoRun();
                    MessageBox.Show("The application has been removed from autorun");
                }
                else
                    MessageBox.Show("The application is not in autorun");
            }
        }

        private void SwitchTheme(object sender, RoutedEventArgs e)
        {
            //Switches theme to selected
            MenuItem theme = sender as MenuItem;
            Application.Current.Resources.MergedDictionaries.Clear();
            if (theme.Header.ToString() == "Standart")
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/StandardTheme.xaml", UriKind.Relative) });
            else if (theme.Header.ToString() == "Light")
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative) });
        }
    }
}