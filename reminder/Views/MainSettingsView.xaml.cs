
using reminder.Windows;
using System.Windows;
using System.Windows.Controls;

namespace reminder.Views
{
    /// <summary>
    /// Interaction logic for MainSettingsView.xaml
    /// </summary>
    public partial class MainSettingsView : UserControl
    {
        private AutoRunManager autoRunManager = new AutoRunManager("ToDoList");

        public MainSettingsView()
        {
            InitializeComponent();
            Properties.Settings.Default.Reload();
        }

        private void AutorunOptionChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.Autorun)
            {
                if (!autoRunManager.ManageAutorun(true)) { new MessageWindow("Unexpected Error", Values.MessageValues.MessageIcon.ERROR); };
            }
            else
            {
                if (!autoRunManager.ManageAutorun(false)) { new MessageWindow("Unexpected Error", Values.MessageValues.MessageIcon.ERROR); };
            }
        }
    }
}
