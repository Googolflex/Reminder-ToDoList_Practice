using reminder.Values;
using reminder.Windows;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace reminder
{
    public partial class EditWindow : Window
    {
        public TaskItem editedTask { get; private set; }

        TasksManager tasksManager = new TasksManager();
        public EditWindow(TaskItem task)
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Owner.Effect = new BlurEffect { Radius = 7 };

            editedTask = task;
            nameBox.Text = task.Name;
            deskBox.Text = task.Desсription;
            timeBox.Text = task.FirstTime.ToString();
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text != String.Empty && deskBox.Text != String.Empty)
            {
                editedTask = tasksManager.editTask(editedTask, nameBox.Text, deskBox.Text, Convert.ToDateTime(timeBox.Text));
                editedTask.IsReminded = editedTask.FirstTime <= DateTime.Now;
                this.DialogResult = true;
            }
            else
            {
                if (nameBox.Text == String.Empty)
                {
                    nameWarning.Content = "Name can`t be empty";
                    nameWarning.Visibility = Visibility.Visible;
                    await Task.Delay(1000);
                    nameWarning.Visibility = Visibility.Hidden;
                    nameWarning.Content = String.Empty;
                }
                else if (deskBox.Text == String.Empty)
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
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Effect = null;
            Thread.Sleep(30);
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
