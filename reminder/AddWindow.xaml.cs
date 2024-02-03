using System;
using System.Windows;

namespace reminder
{
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        public string TaskName
        {
            get { return nameBox.Text; }
        }

        public string TaskDescription
        {
            get { return deskBox.Text; }
        }

        public DateTime TaskTime
        {
            get { return Convert.ToDateTime(timeBox.Text); }
        }

        public DateTime TaskTime2
        {
            get { if (timeBox2.Text != null)
                    return Convert.ToDateTime(timeBox2.Text);
                else
                    return Convert.ToDateTime(timeBox.Text);
            }
        }

        public bool IsTimeInterval
        {
            get { return Convert.ToBoolean(timeIntervalCheckBox.IsChecked); }
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {

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
