using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reminder
{
    public class TaskItem : INotifyPropertyChanged
    {
        public string name;
        public string deskription;
        public DateTime time;
        public bool isChecked;
        public bool isReminded;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Deskription
        {
            get { return deskription; }
            set
            {
                if (deskription != value)
                {
                    deskription = value;
                    OnPropertyChanged(nameof(Deskription));
                }
            }
        }

        public DateTime Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }


        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
        public bool IsReminded
        {
            get { return isReminded; }
            set
            {
                if (isReminded != value)
                {
                    isReminded = value;
                    OnPropertyChanged(nameof(IsReminded));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
