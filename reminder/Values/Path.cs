using System;

namespace reminder
{
    public class Path
    {
        public string FilePath = $"Tasks/{DateTime.Now.ToShortDateString()}.xml";
        public string AutoRunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    }
}
