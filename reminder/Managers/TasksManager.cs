using System;
using System.Collections.ObjectModel;
using System.IO;

namespace reminder
{
    public class TasksManager
    {

        private ObservableCollection<TaskItem> allTasks = new ObservableCollection<TaskItem>();
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
        }
        public TaskItem AddNewTask(string name, string desсription, DateTime firstTime, string group)
        {
            TaskItem newTask = new TaskItem
            {
                Name = name,
                Desсription = desсription,
                FirstTime = firstTime,
                TimeToShow = dayOfWeek.GetDayOfWeekAndTime(firstTime),
                Group = group
            };
            return newTask;
        }

        public void DeleteTask(TaskItem task)
        {
            allTasks.Remove(task);
        }

        public TaskItem editTask(TaskItem task, string editedName, string editedDesc, DateTime editedFstTime)
        {
            task.Name = editedName;
            task.Desсription = editedDesc;
            task.FirstTime = editedFstTime;
            task.TimeToShow = dayOfWeek.GetDayOfWeekAndTime(task.FirstTime);

            return task;
        }

        public ObservableCollection<TaskItem> correctTasksShowTime(ObservableCollection<TaskItem> tasks)
        {
            foreach (TaskItem item in tasks)
            {
                item.TimeToShow = dayOfWeek.GetDayOfWeekAndTime(item.FirstTime);
            }
            return tasks;
        }

        public ObservableCollection<TaskItem> loadTasksFromXml()
        {
            allTasks = xmlManager.DeserializeFromXml<ObservableCollection<TaskItem>>(path.TasksPath);
            return allTasks;
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

        public ObservableCollection<TaskItem> searchInTasks(string request, string group)
        {
            ObservableCollection<TaskItem> temp = sortTasksByGroup(group);
            ObservableCollection <TaskItem> searchResult = new ObservableCollection<TaskItem>();

            foreach (TaskItem item in temp)
            {
                if (item.Name.ToLower().Contains(request.ToLower()) || item.Desсription.ToLower().Contains(request.ToLower()))
                {
                    searchResult.Add(item);
                }
            }

            return searchResult;
        }

        public ObservableCollection<TaskItem> getAllTasks()
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
