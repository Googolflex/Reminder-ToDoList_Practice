using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reminder
{
    public class Path
    {
        public string FilePath = $"Tasks/{DateTime.Now.ToShortDateString()}.xml";
        public string AutoRunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    }
}
