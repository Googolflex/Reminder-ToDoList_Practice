using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace reminder
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            Path path = new Path();
            XmlManager xmlManager = new XmlManager();
            TasksManager tasksManager = new TasksManager();
            ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>();

            if(!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(path.TasksPath))
            {
                ObservableCollection<TaskItem> temp = new ObservableCollection<TaskItem>();
                xmlManager.SerializeToXml(path.TasksPath, temp);
            }

            if (!File.Exists(path.GroupsPath))
            {
                ObservableCollection<GroupItem> temp = new ObservableCollection<GroupItem>();
                xmlManager.SerializeToXml(path.GroupsPath, temp);
            }

            tasks = tasksManager.loadTasksFromXml();
            tasks = tasksManager.correctTasksShowTime(tasks);

            xmlManager.SerializeToXml(path.TasksPath, tasks);

            ApplyTheme(reminder.Properties.Settings.Default.Theme);
        }

        private static void ApplyTheme(string theme)
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, new ResourceDictionary { Source = new Uri($"Themes/{ reminder.Properties.Settings.Default.Theme}.xaml", UriKind.Relative) });
        }
    }
}
