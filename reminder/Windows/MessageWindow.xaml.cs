using reminder.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Principal;
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
using System.Xaml;
using System.Xml.XPath;
using Xceed.Wpf.AvalonDock.Themes;

namespace reminder.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        private MessageValues values = new MessageValues();

        public MessageWindow(string messageBody, MessageValues.MessageIcon icon)
        {
            InitializeComponent();

            try {
                this.Owner = Application.Current.MainWindow;
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                this.Owner.Effect = new BlurEffect { Radius = 7 };
            }
            catch (Exception)
            { }
            finally
            {
                this.Left = (SystemParameters.WorkArea.Width / 2) - (this.Width / 2);
                this.Top = (SystemParameters.WorkArea.Height / 2) - (this.Height / 2);
            }     

            Body.Text = messageBody;
            setMessageIcon(icon);

            if (icon == MessageValues.MessageIcon.QUESTION)
                QuestionWindow();

        }

        private void setMessageIcon(MessageValues.MessageIcon icon)
        {
            switch(icon)
            {
                case MessageValues.MessageIcon.INFO:
                    WindowIcon.Content = Application.Current.Resources["InfoIcon"];
                    break;
                case MessageValues.MessageIcon.ERROR:
                    WindowIcon.Content = Application.Current.Resources["ErrorIcon"];
                    break;
                case MessageValues.MessageIcon.WARNING:
                    WindowIcon.Content = Application.Current.Resources["WarningIcon"];
                    break;
                case MessageValues.MessageIcon.QUESTION:
                    WindowIcon.Content = Application.Current.Resources["QuestionIcon"];
                    break;
            }
        }

        private void QuestionWindow()
        {
            YesButton.IsEnabled = true;
            YesButton.Visibility = Visibility.Visible;
            NoOrOkButton.ContentStringFormat = "No";
            NoOrOkButton.HorizontalAlignment = HorizontalAlignment.Right;
            NoOrOkButton.Margin = new Thickness(0,0,20,0);
        }

        private void NoOrOkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Effect = null;
                Thread.Sleep(30);
            }
        }
    }
}
