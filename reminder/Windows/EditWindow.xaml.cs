using System;
using System.Windows;

namespace reminder
{
    public partial class EditWindow : Window
    {
        public TaskItem editedTask { get; private set; }

        TasksManager tasksManager = new TasksManager();
        public EditWindow(TaskItem task)
        {
            InitializeComponent();
            editedTask = task;
            nameBox.Text = task.Name;
            deskBox.Text = task.Desсription;
            timeBox.Text = task.FirstTime.ToString();
            if (task.SecondTime != DateTime.MinValue)
            {
                timeLabel2.Visibility = Visibility.Visible;
                timeBox2.Visibility = Visibility.Visible;
                timeBox2.Text = task.SecondTime.ToString();
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            editedTask = tasksManager.editTask(editedTask, nameBox.Text, deskBox.Text, Convert.ToDateTime(timeBox.Text), Convert.ToDateTime(timeBox2.Text));
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
