namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Ookii.Dialogs.Wpf;
    using System.Windows.Input;

    public class ViewModel: INotifyPropertyChanged
    {
        private string path;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            this.BrowseCommand = new RelayCommand(_ => this.TryCatch(() => this.BrowsePath()));
        }

        private void BrowsePath()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Please select a folder.";
            dialog.UseDescriptionForTitle = true;

            if ((bool)dialog.ShowDialog())
            {
                this.path = dialog.SelectedPath;
                this.OnPropertyChanged(nameof(this.Path));
            }
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

        public ObservableCollection<Exception> Exceptions { get; } = new ObservableCollection<Exception>();

        public void TryCatch(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                this.Exceptions.Add(e);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
