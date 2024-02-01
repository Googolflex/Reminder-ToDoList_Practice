using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace reminder
{
    public partial class WindowToThePast : Window
    {
        ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();

        string filepath;
        public WindowToThePast(string path)
        {
            InitializeComponent();

            filepath = path;

            taskItems = DeserializeFromXml<ObservableCollection<TaskItem>>(filepath);
            taskBox.ItemsSource = taskItems;

        }

        private void TaskComplete(object sender, RoutedEventArgs e)
        {
            (sender as CheckBox).IsEnabled = false;
        }

        private void taskBox_LostFocus(object sender, RoutedEventArgs e)
        {
            taskBox.SelectedItem = null;
        }
        static T DeserializeFromXml<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        static void SerializeToXml<T>(string filePath, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SerializeToXml(filepath, taskItems);
            this.Close();
        }
    }
}
