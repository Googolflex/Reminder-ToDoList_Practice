using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Credits.xaml
    /// </summary>
    public partial class CreditsWindow : Window
    {
        public CreditsWindow()
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Owner.Effect = new BlurEffect { Radius = 7 };

            SetInfo();
        }

        private void SetInfo()
        {
            TitleText.Text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title + " for Windows10/11";
            VerText.Text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title
                + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Link.Inlines.Add(Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company 
                + " " + Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Effect = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Thread.Sleep(30);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
