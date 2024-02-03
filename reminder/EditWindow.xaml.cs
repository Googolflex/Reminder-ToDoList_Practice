using System;
using System.Windows;

namespace reminder
{
    public partial class EditWindow : Window
    {
        public string EditedName { get; private set; }
        public string EditedDesk { get; private set; }
        public DateTime FirstEditedDate { get; private set; }

        public DateTime SecondEditedDate { get; private set; }
        public EditWindow(TaskItem task)
        {
            InitializeComponent();
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
            EditedName = nameBox.Text;
            EditedDesk = deskBox.Text;
            FirstEditedDate = Convert.ToDateTime(timeBox.Text);
            if(Convert.ToDateTime(timeBox2.Text) != DateTime.MinValue)
                SecondEditedDate = Convert.ToDateTime(timeBox2.Text);
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
