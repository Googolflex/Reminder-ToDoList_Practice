using System;
using System.Windows;

namespace reminder
{
    public partial class EditWindow : Window
    {
        public string EditedName { get; private set; }
        public string EditedDesk { get; private set; }
        public DateTime EditedDate { get; private set; }
        public EditWindow(string initialName, string initialDesk, string initialDate)
        {
            InitializeComponent();
            nameBox.Text = initialName;
            deskBox.Text = initialDesk;
            timeBox.Text = initialDate;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            EditedName = nameBox.Text;
            EditedDesk = deskBox.Text;
            EditedDate = Convert.ToDateTime(timeBox.Text);
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
