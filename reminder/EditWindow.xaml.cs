using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace reminder
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public string EditedName { get; private set; }
        public string EditedDesk { get; private set; }
        public string EditedDate { get; private set; }
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
            EditedDate = timeBox.Text;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
