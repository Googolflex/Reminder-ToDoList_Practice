﻿using System;
using System.Windows;

namespace reminder
{
    public partial class AddWindow : Window
    {

        TasksManager tasksManager = new TasksManager();

        public TaskItem newItem
        {
            get { return tasksManager.CreateNewTaskItem(TaskName, TaskDescription, TaskTime); }
        }

        public AddWindow()
        {
            InitializeComponent();
        }

        public string TaskName
        {
            get { return nameBox.Text; }
        }

        public string TaskDescription
        {
            get { return deskBox.Text; }
        }

        public DateTime TaskTime
        {
            get { return Convert.ToDateTime(timeBox.Text); }
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            timeBox2.Visibility = Visibility.Visible;
            timeLabel2.Visibility = Visibility.Visible;
            timeLabel1.Content = "From:";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            timeLabel1.Content = "Task Time:";
            timeBox2.Text = string.Empty;
            timeBox2.Visibility = Visibility.Hidden;
            timeLabel2.Visibility = Visibility.Hidden;
        }
    }
}
