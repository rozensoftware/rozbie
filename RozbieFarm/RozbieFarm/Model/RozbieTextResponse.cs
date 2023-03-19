using System.ComponentModel;

namespace RozbieFarm.Model
{
    public class RozbieTextResponse : INotifyPropertyChanged
    {
        public string? _txt;

        public string? Txt
        {
            get { return _txt; }
            set
            {
                if (value is not null)
                {
                    _txt = value;
                    OnPropertyChanged(nameof(Txt));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
