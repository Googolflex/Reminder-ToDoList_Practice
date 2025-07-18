﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.AvalonDock.Controls;
using System.Linq;
using reminder.Windows;
using reminder.Values;
using reminder.CustomControls;
using System.Collections.Generic;

namespace reminder
{
    public partial class MainWindow : Window
    {
        //Creating collection for storing tasks based on task item model
        private ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();

        //Creating collection for storing groups
        private ObservableCollection<GroupItem> groupItems = new ObservableCollection<GroupItem>();

        private string selectedGroup;

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


        //Number of unviewed notifications to display in the notification icon
        private int unseenNotify = 0;

        //Parameters for work with window states
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
            groupItems = groupsManager.loadGroupsFromXml();

            All_Tasks.IsSelected = true;

            taskBox.ItemsSource = tasksManager.getAllTasks();
            CustomGroups.ItemsSource = groupItems;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //At each tick, checks whether any notifications need to be sent
            CheckTaskTime();
        }

        private void CheckTaskTime()
        {
            //If time in task < time now and task not remindet yet then send an notification
            foreach (TaskItem taskItem in taskItems)
            {
                if (DateTime.Now >= taskItem.FirstTime && (!taskItem.IsReminded && !taskItem.IsComplete))
                {
                    unseenNotify++;

                    //Send windows notify
                    taskbarIcon.ShowBalloonTip(taskItem.Name, taskItem.Desсription, BalloonIcon.Info);
                    taskItem.IsReminded = true;

                    //Send in app notify
                    NotificationItem notify = new NotificationItem();
                    notify.Header = taskItem.Name;
                    NotTable.AddNotification(notify);
                    notiCount.Text = unseenNotify.ToString();

                    tasksManager.saveTasksToXml();

                    if(notiCounterEllipse.Visibility != Visibility.Visible)
                    {
                        notiCounterEllipse.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        //Open window for add an task.
        private void AddTask(object sender, MouseButtonEventArgs e)
        {
            AddWindow addWindow = new AddWindow(selectedGroup);
            addWindow.ShowDialog();
            if (addWindow.DialogResult == true)
            {
                taskItems.Add(addWindow.NewTask);
                tasksManager.UpdateAllTasks(taskItems);
                taskItems = tasksManager.sortTasksByGroup(selectedGroup);
                tasksManager.saveTasksToXml();
            }
            taskBox.ItemsSource = taskItems;
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
                tasksManager.DeleteTask(selectedTask);
                tasksManager.saveTasksToXml();
            }
        }

        private void EditTask(object sender, MouseButtonEventArgs e)
        {
            //Opens edit window for selected task when edit button is pressed and selected task is active.
            if (taskBox.SelectedItem != null && !(taskBox.SelectedItem as TaskItem).IsComplete)
            {
                TaskItem selectedTask = (TaskItem)taskBox.SelectedItem;
                EditWindow editWindow = new EditWindow(selectedTask);
                editWindow.ShowDialog();

                if (editWindow.DialogResult == true)
                {
                    selectedTask = editWindow.editedTask;
                    tasksManager.saveTasksToXml();
                }                
            }
        }

        private void taskBarOptions(object sender, RoutedEventArgs e)
        {
            //Defines what option is clicked in taskbar
            string option = (sender as MenuItem).HeaderStringFormat.ToString();
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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (closingFromMenuItem && !isClosingHandled)
            {
                e.Cancel = true;
                MessageWindow message = new MessageWindow(MessageValues.OnClosingQuestionText, MessageValues.MessageIcon.QUESTION);
                message.ShowDialog();
                bool result = (bool)message.DialogResult;
                if (result == false)
                {
                    closingFromMenuItem = false;
                }
                else
                {
                    tasksManager.saveTasksToXml();
                    groupsManager.saveGroupsToXml(groupItems);
                    isClosingHandled = true;
                    e.Cancel = false;
                }
            }
            else
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

        private void AddGroup(object sender, RoutedEventArgs e)
        {
            PlaceholderTextBox box = (PlaceholderTextBox)AddMenu.FindVisualChildren<PlaceholderTextBox>().First();
            groupItems.Add(groupsManager.NewGroupItem(box.Text, groupItems));

            box.Clear();

            AddMenu.IsOpen = false;

            groupsManager.saveGroupsToXml(groupItems);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddMenu.PlacementTarget = (IconButton)sender;
            AddMenu.Placement = PlacementMode.Bottom;
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

            selectedGroup = item.Tag.ToString();

            taskItems = tasksManager.sortTasksByGroup(selectedGroup);

            SearchBox.ClearInput();

            taskBox.ItemsSource = taskItems;
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
                WindowBorder.CornerRadius = new CornerRadius(0);
                LeftMenuBorder.CornerRadius = new CornerRadius(0);
                TopMenuBorder.CornerRadius = new CornerRadius(0);
            }
            else
            {
                this.Left = 180;
                this.Top = 150;
                this.Width = 1095;
                this.Height = 788;
                WindowBorder.CornerRadius = new CornerRadius(10);
                LeftMenuBorder.CornerRadius = new CornerRadius(0, 0, 0, 10);
                TopMenuBorder.CornerRadius = new CornerRadius(10, 10, 0, 0);
            }

            isMaximazed = !isMaximazed;
        }

        private void MinWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState= WindowState.Minimized;
        }

        private void Button_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            OptionsMenu.PlacementTarget = MenuButton;
            OptionsMenu.Placement = PlacementMode.Bottom;
            MenuButton.ContextMenu.IsOpen = true;
        }

        private void TrayDoubleClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
            this.ShowInTaskbar = true;
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            var option = (MenuItem)sender;
            switch (option.Header)
            {
                case "Options":
                    SettingsWindow setWindow = new SettingsWindow();
                    setWindow.ShowDialog();
                    break;
                case "Credits":
                    CreditsWindow credWindow = new CreditsWindow();
                    credWindow.ShowDialog();
                    break;
                default:
                    MessageWindow message = new MessageWindow("Unexpected error", MessageValues.MessageIcon.ERROR);
                    message.ShowDialog();
                    break;
            }
        }

        private void ScrollWhenMouseOver(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            if (scroll != null)
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }

        private void NotButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotTable.IsOpened()) 
            {
                NotTable.Visibility = Visibility.Collapsed;
                NotTable.ChangeVisibility();
            } 
            else 
            {
                NotTable.Visibility = Visibility.Visible;
                NotTable.ChangeVisibility();
                NotTable.IsEnabled = true;
                unseenNotify = 0;
                notiCounterEllipse.Visibility = Visibility.Collapsed;
            }
        }

        private async void NotButton_LostFocus(object sender, RoutedEventArgs e)
        {
            await Task.Delay(200);

            NotTable.Visibility = Visibility.Collapsed;
            NotTable.IsEnabled = false;
            NotTable.ChangeVisibility();
        }

        private async void AddMenu_LostFocus(object sender, RoutedEventArgs e)
        {
            await Task.Delay(2000);
            PlaceholderTextBox gpBox = (PlaceholderTextBox)AddMenu.FindVisualChildren<PlaceholderTextBox>().First();

            gpBox.Clear();
        }

        private void TasksOptionsMenuOpen(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            but.ContextMenu.PlacementTarget = but;
            but.ContextMenu.Placement = PlacementMode.Bottom;
            but.ContextMenu.IsOpen = true;
        }

        private void ClearCompletedTasks(object sender, RoutedEventArgs e)
        {
            var temp = new ObservableCollection<TaskItem>();
            foreach (TaskItem task in taskItems)
            {
                if(task.IsComplete)
                {
                    temp.Add(task);
                }
            }
            foreach (TaskItem task in temp)
            {
                taskItems.Remove(task);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text != "" || SearchBox.Text != string.Empty)
            {
                taskItems = tasksManager.searchInTasks(SearchBox.Text, selectedGroup);
                taskBox.ItemsSource = taskItems;
            }
            else
            {
                taskItems = tasksManager.sortTasksByGroup(selectedGroup);
                taskBox.ItemsSource = taskItems;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            if (NotTable.IsOpened())
            {
                NotTable.Visibility = Visibility.Collapsed;
                NotTable.ChangeVisibility();
            }
        }

        //Deletes selected group when delete button is pressed 
        private void DeleteGroup(object sender, RoutedEventArgs e)
        {
            GroupItem selectedGroup = (GroupItem)CustomGroups.SelectedItem;
            if (CustomGroups.SelectedItem != null)
            {
                MessageWindow mes = new MessageWindow("Do you want to delete all tasks in a group?", MessageValues.MessageIcon.QUESTION);
                mes.ShowDialog();
                if (mes.Result != MessageValues.MessageResult.None)
                {
                    if (mes.Result == MessageValues.MessageResult.Yes)
                    {
                        List<TaskItem> temp = new List<TaskItem>();
                        foreach (TaskItem task in taskItems)
                        {
                            if (task.Group == selectedGroup.Name)
                            {
                                temp.Add(task);
                            }
                        }
                        foreach (TaskItem item in temp)
                        {
                            tasksManager.DeleteTask(item);
                        }
                    }
                    else if (mes.Result == MessageValues.MessageResult.No)
                    {
                        foreach (TaskItem task in taskItems)
                        {
                            if (task.Group == selectedGroup.Name)
                            {
                                task.Group = "All Tasks";
                            }
                        }
                    }
                    groupItems.Remove(selectedGroup);
                    tasksManager.saveTasksToXml();
                    groupsManager.saveGroupsToXml(groupItems);
                    All_Tasks.IsSelected = true;
                }
            }
        }
    }
}