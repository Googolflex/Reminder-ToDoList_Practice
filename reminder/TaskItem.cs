using System;
using System.ComponentModel;

namespace reminder
{
    public class TaskItem : INotifyPropertyChanged
    {
        public string name;
        public string description;
        public DateTime firstTime;
        public DateTime secondTime;
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

        public DateTime FirstTime
        {
            get { return firstTime; }
            set
            {
                if (firstTime != value)
                {
                    firstTime = value;
                    OnPropertyChanged(nameof(FirstTime));
                }
            }
        }

        public DateTime SecondTime
        {
            get { return secondTime; }
            set
            {
                if (secondTime != value)
                {
                    secondTime = value;
                    OnPropertyChanged(nameof(SecondTime));
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
