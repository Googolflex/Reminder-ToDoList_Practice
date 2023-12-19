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


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }
    }
}
