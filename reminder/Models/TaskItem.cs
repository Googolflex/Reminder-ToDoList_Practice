using System;
using System.ComponentModel;

namespace reminder
{
    public class TaskItem : INotifyPropertyChanged
    {
        private static int id = 0;

        private string name;
        private string description;
        private DateTime firstTime;
        private string timeToShow;
        private string group = "";
        private bool isComplete = false;
        private bool isReminded = false;

        public int Id
        {
            get { return id++; }
        }

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

        public string Group
        {
            get { return group; }
            set
            {
                if (group != value)
                {
                    group = value;
                    OnPropertyChanged(nameof(Group));
                }
            }
        }

        public bool IsComplete
        {
            get { return isComplete; }
            set
            {
                if (isComplete != value)
                {
                    isComplete = value;
                    OnPropertyChanged(nameof(isComplete));
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
