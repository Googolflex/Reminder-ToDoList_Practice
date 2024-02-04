using System;
using System.Windows;

namespace reminder
{
    public partial class EditWindow : Window
    {
        public TaskItem editedTask { get; private set; }
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
            editedTask.Name = nameBox.Text;
            editedTask.Desсription = deskBox.Text;
            editedTask.FirstTime = Convert.ToDateTime(timeBox.Text);
            if (Convert.ToDateTime(timeBox2.Text) != DateTime.MinValue)
            {
                editedTask.SecondTime = Convert.ToDateTime(timeBox2.Text);
                editedTask.TimeToShow = $"{editedTask.FirstTime.ToShortDateString()} {editedTask.FirstTime.ToShortTimeString()} - {editedTask.SecondTime.ToShortDateString()} {editedTask.SecondTime.ToShortTimeString()}";
            }
            else
                editedTask.TimeToShow = $"{editedTask.FirstTime.ToShortDateString()} {editedTask.FirstTime.ToShortTimeString()}";
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
