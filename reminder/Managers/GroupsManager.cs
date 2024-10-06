using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace reminder
{
    public class GroupsManager
    {
        public bool CheckColor(string color, ObservableCollection<GroupItem> list)
        {
            foreach (GroupItem item in list)
            {
                if (item.GroupColor == color)
                    return false;
            }
            return true;
        }

        public GroupItem NewGroupItem(string name, ObservableCollection<GroupItem> groupItems)
        {
            Random rnd = new Random();
            byte r = 0, g = 0, b = 0;
            while (((r < 150 || b < 150) && (r + g + b < 320)) && CheckColor(new SolidColorBrush(Color.FromRgb(r, g, b)).Color.ToString(), groupItems))
            {
                r = (byte)rnd.Next(1, 255);
                b = (byte)rnd.Next(1, 255);
                g = (byte)rnd.Next(1, 255);
            }
            string color = new SolidColorBrush(Color.FromRgb(r, g, b)).Color.ToString();
            GroupItem item = new GroupItem
            {
                Name = name,
                GroupColor = color
            };
            return item;
        }
    }
}
