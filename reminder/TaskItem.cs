using System;
using System.ComponentModel;

namespace reminder
{
    public class TaskItem : INotifyPropertyChanged
    {
        public string name;
        public string description;
        public DateTime time;
        public string timeToShow;
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

        public string Desсription
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Desсription));
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

        public string TimeToShow
        {
            get { return timeToShow; }
            set
            {
                if (timeToShow != value)
                {
                    timeToShow = value;
                    OnPropertyChanged(nameof(TimeToShow));
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
