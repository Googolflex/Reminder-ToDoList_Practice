using System;
using System.Collections.ObjectModel;
using System.IO;

namespace reminder
{
    public class TasksManager
    {

        ObservableCollection<String> previousDaysCollettion = new ObservableCollection<String>();
        XmlManager xmlManager = new XmlManager();
        DateToDayOfWeek dayOfWeek = new DateToDayOfWeek();
        Path path = new Path();
        public TaskItem CreateNewTaskItem( string name, string desсription, DateTime firstTime)
        {
            TaskItem newTask = new TaskItem();
            
            newTask = new TaskItem
            {
                Name = name,
                Desсription = desсription,
                FirstTime = firstTime,
                TimeToShow = $"{dayOfWeek.WhatsDayOfWeek(firstTime)} {firstTime.ToShortTimeString()}",
                IsComplete = false,
                IsReminded = false
            };
            return newTask;
        }

        public TaskItem editTask(TaskItem task, string editedName, string editedDesc, DateTime editedFstTime, DateTime editedSecTime)
        {
            task.Name = editedName;
            task.Desсription = editedDesc;
            task.FirstTime = editedFstTime;
            task.TimeToShow = $"{dayOfWeek.WhatsDayOfWeek(task.FirstTime)} {task.FirstTime.ToShortTimeString()}";

            return task;
        }

        public ObservableCollection<GroupItem> loadGroupsFromXml()
        {
            return xmlManager.DeserializeFromXml<ObservableCollection<GroupItem>>(path.GroupsPath);
        }

        public ObservableCollection<TaskItem> loadTasksFromXml()
        {
            return xmlManager.DeserializeFromXml<ObservableCollection<TaskItem>>(path.TasksPath);
        }

        public ObservableCollection<String> previousDaysTasks() 
        {
            string[] strings = new string[Directory.GetFiles("Tasks").Length];
            strings = Directory.GetFiles("Tasks");
            foreach (string file in strings)
            {
                string temp = file.Remove(0, 6).Remove(10, 4);
                if (temp != DateTime.Now.ToShortDateString())
                    previousDaysCollettion.Add(temp);
            }
            return previousDaysCollettion;
        }

        public ObservableCollection<TaskItem> tasksToRemoveCollection(ObservableCollection<TaskItem> taskItems)
        {
            ObservableCollection<TaskItem> tasksToRemove = new ObservableCollection<TaskItem>();
            foreach (TaskItem item in taskItems)
            {
                if (item.IsComplete)
                    tasksToRemove.Add(item);
            }
            return tasksToRemove;
        }
    }
}
