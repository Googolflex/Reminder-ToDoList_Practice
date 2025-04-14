using reminder.CustomControls;
using reminder.Values;
using reminder.Windows;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace reminder
{
    public partial class AddWindow : Window
    {

        private TasksManager tasksManager = new TasksManager();
        private string _taskGroup;

        public AddWindow(string group)
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Owner.Effect = new BlurEffect { Radius = 7 };

            timeBox.Text = DateTime.Now.ToString();

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Effect = null;
        }

        private async void InputExceededMaxLength(object sender, EventArgs e)
        {
            if (sender == nameBox)
            {
                nameWarning.Visibility = Visibility.Visible;
                await Task.Delay(600);
                nameWarning.Visibility = Visibility.Hidden;
            }
            else if (sender == deskBox)
            {
                descriptionWarning.Visibility = Visibility.Visible;
                await Task.Delay(600);
                descriptionWarning.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageWindow erWin = new MessageWindow(MessageValues.ErrorText, MessageValues.MessageIcon.ERROR);
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
