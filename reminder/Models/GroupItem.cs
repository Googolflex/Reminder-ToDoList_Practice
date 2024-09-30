using System.ComponentModel;
using System.Windows.Media;

namespace reminder
{
    public class GroupItem : INotifyPropertyChanged
    {
        private SolidColorBrush _groupColor;
        private string _name;

        public SolidColorBrush GroupColor
        {
            get { return _groupColor; }
            set
            {
                if (_groupColor != value)
                {
                    _groupColor = value;
                    OnPropertyChanged(nameof(GroupColor));
                }
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
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
