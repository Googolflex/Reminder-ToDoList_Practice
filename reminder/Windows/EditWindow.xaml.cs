using System;
using System.Threading.Tasks;
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
            if (editedTask.SecondTime != DateTime.MinValue && Convert.ToDateTime(timeBox.Text) < Convert.ToDateTime(timeBox2.Text))
            {
                editedTask = tasksManager.editTask(editedTask, nameBox.Text, deskBox.Text, Convert.ToDateTime(timeBox.Text), Convert.ToDateTime(timeBox2.Text));
                this.DialogResult = true;
            }
            else if (editedTask.SecondTime != DateTime.MinValue && Convert.ToDateTime(timeBox.Text) > Convert.ToDateTime(timeBox2.Text))
                MessageBox.Show("Enter the correct time interval", "Error");
            else
            {
                editedTask = tasksManager.editTask(editedTask, nameBox.Text, deskBox.Text, Convert.ToDateTime(timeBox.Text), Convert.ToDateTime(timeBox2.Text));
                this.DialogResult = true;
            }
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
