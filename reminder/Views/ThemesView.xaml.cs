using System;
using System.Windows;
using System.Windows.Controls;

namespace reminder.Views
{
    /// <summary>
    /// Interaction logic for ThemesView.xaml
    /// </summary>
    public partial class ThemesView : UserControl
    {
        public ThemesView()
        {
            InitializeComponent();
        }

        private void StTheme_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Properties.Settings.Default.Theme = "StandardTheme";
            Properties.Settings.Default.Save();
            Application.Current.Resources.MergedDictionaries.Insert(0, new ResourceDictionary 
                { Source = new Uri($"Themes/{Properties.Settings.Default.Theme}.xaml", UriKind.Relative) });
        }

        private void LghtTheme_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Properties.Settings.Default.Theme = "LightTheme";
            Properties.Settings.Default.Save();
            Application.Current.Resources.MergedDictionaries.Insert(0, new ResourceDictionary 
                { Source = new Uri($"Themes/{Properties.Settings.Default.Theme}.xaml", UriKind.Relative) });
        }
    }
}
