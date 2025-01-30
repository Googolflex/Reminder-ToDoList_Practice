using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace reminder
{
    public partial class EditWindow : Window
    {
        public TaskItem editedTask { get; private set; }

        TasksManager tasksManager = new TasksManager();
        public EditWindow(TaskItem task)
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Owner.Effect = new BlurEffect { Radius = 7 };

            editedTask = task;
            nameBox.Text = task.Name;
            deskBox.Text = task.Desсription;
            timeBox.Text = task.FirstTime.ToString();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            editedTask = tasksManager.editTask(editedTask, nameBox.Text, deskBox.Text, Convert.ToDateTime(timeBox.Text));
            this.DialogResult = true;
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Effect = null;
            Thread.Sleep(30);
        }
    }
}
