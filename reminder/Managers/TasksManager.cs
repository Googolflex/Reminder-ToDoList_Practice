using System;
using System.Collections.ObjectModel;
using System.IO;

namespace reminder
{
    public class TasksManager
    {

        public ObservableCollection<TaskItem> allTasks = new ObservableCollection<TaskItem>();
        private XmlManager xmlManager = new XmlManager();
        private DateToDayOfWeek dayOfWeek = new DateToDayOfWeek();
        private Path path = new Path();


        public void UpdateAllTasks(ObservableCollection<TaskItem> tasks)
        {
            foreach (TaskItem item in tasks)
            {
                if (!allTasks.Contains(item))
                {
                    allTasks.Add(item);
                }
            }
            foreach (TaskItem item in allTasks)
                Console.WriteLine(item.Id);
        }
        public TaskItem AddNewTask(string name, string desсription, DateTime firstTime, string group)
        {
            TaskItem newTask = new TaskItem
            {
                Name = name,
                Desсription = desсription,
                FirstTime = firstTime,
                TimeToShow = $"{dayOfWeek.WhatsDayOfWeek(firstTime)} {firstTime.ToShortTimeString()}",
                Group = group
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

        public ObservableCollection<TaskItem> loadTasksFromXml()
        {
            allTasks = xmlManager.DeserializeFromXml<ObservableCollection<TaskItem>>(path.TasksPath);
            return xmlManager.DeserializeFromXml<ObservableCollection<TaskItem>>(path.TasksPath);
        }

        public void saveTasksToXml()
        {
            xmlManager.SerializeToXml(path.TasksPath, allTasks);
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

        public ObservableCollection<TaskItem> sortTasksByGroup(string group)
        {
            ObservableCollection<TaskItem> temp = new ObservableCollection<TaskItem>();
            switch(group)
            {
                case "All Tasks":
                    temp = getAllTasks();
                    break;
                case "Today":
                    temp = getTodaysTasks();
                    break;
                default:
                    temp = getGroupTasks(group);
                    break;
            }
            return temp;
        }

        private ObservableCollection<TaskItem> getAllTasks()
        {
            return allTasks;
        }

        private ObservableCollection<TaskItem> getTodaysTasks()
        {
            ObservableCollection<TaskItem> tasks = allTasks;
            ObservableCollection<TaskItem> sorted = new ObservableCollection<TaskItem>();
            foreach (TaskItem item in tasks)
            {
                if (item.FirstTime.Date == DateTime.Today)
                    sorted.Add(item);
            }
            return sorted;
        }

        private ObservableCollection<TaskItem> getGroupTasks(string group)
        {
            ObservableCollection<TaskItem> tasks = allTasks;
            ObservableCollection<TaskItem> sorted = new ObservableCollection<TaskItem>();
            foreach(TaskItem item in tasks)
            {
                if (item.Group == group)
                    sorted.Add(item);
            }
            return sorted;
        }
    }
}
