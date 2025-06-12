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


        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (TaskName != String.Empty && TaskDescription != String.Empty)
            {
                tasksManager.AddNewTask(TaskName, TaskDescription, TaskTime, Group);
                this.DialogResult = true;
            }
            else
            {
                if (TaskName == String.Empty)
                {
                    nameWarning.Content = "Name can`t be empty";
                    nameWarning.Visibility = Visibility.Visible;
                    await Task.Delay(1000);
                    nameWarning.Visibility = Visibility.Hidden;
                    nameWarning.Content = String.Empty;
                }
                else if (TaskDescription == String.Empty)
                {
                    descriptionWarning.Content = "Description can`t be empty";
                    descriptionWarning.Visibility = Visibility.Visible;
                    await Task.Delay(1000);
                    descriptionWarning.Visibility = Visibility.Hidden;
                    descriptionWarning.Content = String.Empty;
                }
            }
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
                nameWarning.Content = "Name too long";
                nameWarning.Visibility = Visibility.Visible;
                await Task.Delay(600);
                nameWarning.Visibility = Visibility.Hidden;
                nameWarning.Content = String.Empty;
            }
            else if (sender == deskBox)
            {
                descriptionWarning.Content = "Description too long";
                descriptionWarning.Visibility = Visibility.Visible;
                await Task.Delay(600);
                descriptionWarning.Visibility = Visibility.Hidden;
                descriptionWarning.Content = String.Empty;
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
