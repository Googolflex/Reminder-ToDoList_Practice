using System;
using System.Windows;

namespace reminder
{
    public partial class AddWindow : Window
    {

        private TasksManager tasksManager = new TasksManager();
        private string _taskGroup;

        public AddWindow(string group)
        {
            InitializeComponent();

            if (group != "All Tasks" || group != "Today")
                _taskGroup = group;
            else
                _taskGroup = "";
        }

        public TaskItem NewTask
        {
            get { return tasksManager.AddNewTask(TaskName, TaskDescription, TaskTime, Group); }
        }

        private string TaskName
        {
            get { return nameBox.Text; }
        }

        private string TaskDescription
        {
            get { return deskBox.Text; }
        }

        private DateTime TaskTime
        {
            get { return Convert.ToDateTime(timeBox.Text); }
        }

        private string Group
        {
            get { return _taskGroup; }
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            tasksManager.AddNewTask(TaskName, TaskDescription, TaskTime, Group);
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            timeBox2.Visibility = Visibility.Visible;
            timeLabel2.Visibility = Visibility.Visible;
            timeLabel1.Content = "From:";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            timeLabel1.Content = "Task Time:";
            timeBox2.Text = string.Empty;
            timeBox2.Visibility = Visibility.Hidden;
            timeLabel2.Visibility = Visibility.Hidden;
        }
    }
}
