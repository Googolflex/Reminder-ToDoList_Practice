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
using System.Windows.Media;
using System.Threading;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.AvalonDock.Controls;
using System.Linq;

namespace reminder
{
    public partial class MainWindow : Window
    {
        //Creating collection for storing tasks based on task item model
        private ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();

        //Creating collection for storing groups
        private ObservableCollection<GroupItem> groupItems = new ObservableCollection<GroupItem>();

        //Creating class instance for work with autorun
        private AutoRunManager autoRunManager = new AutoRunManager("ToDoList");

        //Creating class instance for work with xml
        private XmlManager xmlManager = new XmlManager();

        //Creating class instance for work with tasks(edit, add new)
        private TasksManager tasksManager = new TasksManager();

        //Creating class instance for work with groups
        private GroupsManager groupsManager = new GroupsManager();

        //Paths to autorun and tasks save file
        private Path path = new Path();

        //Timer
        private DispatcherTimer timer;

        /*Parameters for prevent app closing from window
            When closing invoked from taskbar first parameter sets to true*/
        private bool closingFromMenuItem = false;
        private bool isClosingHandled = false;
        private bool isMaximazed = false;
        private double workingAreaW = SystemParameters.WorkArea.Width;
        private double workingAreaH = SystemParameters.WorkArea.Height;

        public MainWindow()
        {
            this.Closing += MainWindow_Closing;

            InitializeComponent();

            //Timer parameters
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();

            taskItems = tasksManager.loadTasksFromXml();
            groupItems = tasksManager.loadGroupsFromXml();

            taskBox.ItemsSource = taskItems;
            CustomGroups.ItemsSource = groupItems;

            All_Tasks.IsSelected = true;
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
                if (DateTime.Now >= taskItem.FirstTime && (!taskItem.IsReminded && !taskItem.IsComplete))
                {
                    taskbarIcon.ShowBalloonTip(taskItem.Name, taskItem.Desсription, BalloonIcon.Info);
                    taskItem.IsReminded = true;
                }
            }
        }

        private void AddTask(object sender, MouseButtonEventArgs e)
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
            if (taskBox.SelectedItem != null && !(taskBox.SelectedItem as TaskItem).IsComplete)
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

        private void Credits(object sender, RoutedEventArgs e)
        {
            //Opens my github. It made this way cause earlier it was project for college practice and I made it not alone
            string devName = (sender as MenuItem).Header.ToString();
            System.Diagnostics.Process.Start($"https://github.com/{devName}");

        }

        private void taskBarOptions(object sender, RoutedEventArgs e)
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
                    xmlManager.SerializeToXml(path.TasksPath, taskItems);
                    xmlManager.SerializeToXml(path.GroupsPath, groupItems);
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

        private void ClearCompleted(object sender, RoutedEventArgs e)
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

        private void taskBoxLostFocus(object sender, RoutedEventArgs e)
        {
            //When user clicks away from selected task its lost focus
            taskBox.SelectedItem = null;
        }

        //Opens window with old task (for selected day). LEGACY! NEEDS TO BE DELETED!! 
        private void OpenPreviousDay(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            string day = menuItem.Header.ToString();
            WindowToThePast windowToThePast = new WindowToThePast($"Tasks/{day}.xml");
            windowToThePast.Show();
        }

        //Adds app to auto run or removed app from it. Upon completion drops info message
        private void AutorunOptions(object sender, RoutedEventArgs e)
        {
            //Defines selected autorun option and invokes it
            MenuItem senderButton = sender as MenuItem;
            MessageBox.Show(autoRunManager.ManageAutorun(senderButton.Header.ToString()));
        }

        //Switches theme to selected
        private void SwitchTheme(object sender, RoutedEventArgs e)
        {
            MenuItem theme = sender as MenuItem;
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            //Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Insert(0, new ResourceDictionary { Source = new Uri($"Themes/{theme.Header.ToString()}Theme.xaml", UriKind.Relative) });
        }

        private void AddGroup(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)AddMenu.FindVisualChildren<TextBox>().First();

            groupItems.Add(groupsManager.NewGroupItem(box.Text.ToString(), groupItems));
        }

        private void AddButton_Click(object sender, MouseButtonEventArgs e)
        {
            AddMenu.IsOpen = true;
        }

        private void GroupChanged(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            HeaderText.Text = item.Tag.ToString();
            if (item.Tag.ToString() == "All Tasks" || item.Tag.ToString() == "Today")
                CustomGroups.SelectedItem = null;
            else
                DefaultGroups.SelectedItem = null;
        }

        private void TopMenu_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxWindow(object sender, RoutedEventArgs e)
        {
            if (!isMaximazed)
            {
                this.Left = 0;
                this.Top = 0;
                this.Width = workingAreaW;
                this.Height = workingAreaH;
            }
            else
            {
                this.Left = 180;
                this.Top = 150;
                this.Width = 1095;
                this.Height = 788;
            }

            isMaximazed = !isMaximazed;
        }

        private void MinWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState= WindowState.Minimized;
        }
    }
}