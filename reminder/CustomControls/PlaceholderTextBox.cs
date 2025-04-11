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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:reminder.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:reminder.CustomControls;assembly=reminder.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:PlaceholderTextBox/>
    ///
    /// </summary>
    public class PlaceholderTextBox : Control
    {

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PlaceholderTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata(string.Empty));

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(PlaceholderTextBox), new PropertyMetadata(10000));

        public TextWrapping AllowWrapping
        {
            get { return (TextWrapping)GetValue(AllowWrappingProperty); }
            set { SetValue(AllowWrappingProperty, value); }
        }

        public static readonly DependencyProperty AllowWrappingProperty =
            DependencyProperty.Register("AllowWrapping", typeof(TextWrapping), typeof(PlaceholderTextBox), new PropertyMetadata(TextWrapping.NoWrap));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PlaceholderTextBox)d;
            var newText = e.NewValue as string ?? string.Empty;

            int maxLength = control.MaxLength;

            control.IsTextTooLong = control.Text?.Length > maxLength;
            if (control.IsTextTooLong)
            {
                control.Text = newText.Substring(0, maxLength);
            }
            control.IsTextTooLong = control.Text?.Length > maxLength;
        }

        public bool IsTextTooLong
        {
            get { return (bool)GetValue(IsTextTooLongProperty); }
            set { SetValue(IsTextTooLongProperty, value); }
        }

        public static readonly DependencyProperty IsTextTooLongProperty =
            DependencyProperty.Register("IsTextTooLong", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        static PlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(typeof(PlaceholderTextBox)));
        }

        public void Clear()
        {
           Text = string.Empty;
        }
    }
}
