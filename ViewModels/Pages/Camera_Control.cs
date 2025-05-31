using System.ComponentModel;

namespace 이거인가보오.ViewModels.Pages
{
    public partial class E_Data : INotifyPropertyChanged
    {
        private string? _name;
        private int _count;
        private List<float> _acc = new List<float>();

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

         public int Count
    {
        get => _count;
        set
        {
            _count = value;
            OnPropertyChanged(nameof(Count));
        }
    }

    public List<float> Acc
    {
        get => _acc;
        set
        {
            _acc = value;
            OnPropertyChanged(nameof(Acc));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
