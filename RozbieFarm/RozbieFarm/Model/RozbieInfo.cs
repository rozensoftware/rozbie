using System.ComponentModel;

namespace RozbieFarm.Model
{
    public class RozbieInfo : INotifyPropertyChanged
    {
        private string _ip;
        private string? _name;

        public RozbieInfo(string ip, string? name)
        {
            _ip = ip;
            _name = name;
        }

        public string? Name
        {
            get { return _name; }
            set
            {
                if (value is not null)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string IP
        {
            get { return _ip; }
            set
            {
                if (value is not null)
                {
                    _ip = value;
                    OnPropertyChanged(nameof(IP));
                }
            }
        }

        public string FullName => $"{Name} ({IP})";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
