using System;

namespace reminder
{
    public class Path
    {
        public string TasksPath = $"Data/Tasks.xml";
        public string GroupsPath = $"Data/Groups.xml";
        public string AutoRunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    }
}
