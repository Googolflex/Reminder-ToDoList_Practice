using reminder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace reminder.Windows
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Owner.Effect = new BlurEffect { Radius = 7 };

            MainSettingsBut.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            MainSettingsBut.Focus();
        }

        private void MainSettings_Click(object sender, RoutedEventArgs e)
        {
            settingsPres.Content = new MainSettingsView();
        }

        private void ThemesBut_Click(object sender, RoutedEventArgs e)
        {
            settingsPres.Content = new ThemesView();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Effect = null;
            Thread.Sleep(30);
        }
    }
}
