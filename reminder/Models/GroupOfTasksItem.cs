using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace reminder.Models
{
    public class GroupOfTasksItem
    {

        private static int nextId = 0;

        public int id;
        public string name;
        public string pathToTasks;
        public SolidColorBrush groupColor;
        public int Id
        {
            get { return nextId++; }
        }

        public string Name => name;
        public string PathToTasks => pathToTasks;
        public SolidColorBrush GroupColor => groupColor;
    }
}
