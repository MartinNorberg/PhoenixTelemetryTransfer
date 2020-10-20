namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class ViewModel: INotifyPropertyChanged
    {
        private string path;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
        }

        public string Path
        {
            get => this.path;
            set
            {
                if (Equals(this.path, value))
                {
                    return;
                }

                this.path = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand BrowseCommand { get; }

        public ICommand StartCommand { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
