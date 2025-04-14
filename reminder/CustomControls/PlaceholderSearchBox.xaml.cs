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

namespace reminder.CustomControls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class PlaceholderSearchBox : UserControl
    {

        public string Text;

        public event TextChangedEventHandler InputChanged;

        public PlaceholderSearchBox()
        {
            InitializeComponent();
        }

        public void ClearInput()
        {
            ClearSearch(ClearButton, new RoutedEventArgs());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = SearchBox.Text;
            CastOnTextChangedEvent();
        }

        protected virtual void CastOnTextChangedEvent()
        {
            InputChanged?.Invoke(this, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));
        }

        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
        }
    }
}
