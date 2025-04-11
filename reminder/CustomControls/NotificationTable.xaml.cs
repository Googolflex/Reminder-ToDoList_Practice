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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Controls;

namespace reminder.CustomControls
{
    /// <summary>
    /// Interaction logic for NotificationTable.xaml
    /// </summary>
    public partial class NotificationTable : UserControl
    {

        private bool opened = false;

        public NotificationTable()
        {
            InitializeComponent();
        }

        public void AddNotification(NotificationItem notification)
        {
            NotiTable.Children.Add(notification);
        }

        public bool IsOpened()
        {
            return opened;
        }

        public void ChangeVisibility()
        {
            opened = !opened;
        }

        private void ScrollWhenMouseOver(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            if (scroll != null)
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }

        private void ClearNotifies(object sender, RoutedEventArgs e)
        {
            NotiTable.Children.Clear();
        }
    }
}
