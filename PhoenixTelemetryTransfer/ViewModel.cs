namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Ookii.Dialogs.Wpf;
    using System.Windows.Input;
    using System.Configuration;

    public class ViewModel: INotifyPropertyChanged
    {
        private string path;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            this.TryCatch(() => this.LoadConfig());
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
                this.UpdateSetting("Path", this.Path);
            }
        }

        private void LoadConfig()
        {
            this.Path = ConfigurationManager.AppSettings["Path"].ToString();
        }

        private void UpdateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
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

        public ObservableCollection<Channel> Channels { get; } = new ObservableCollection<Channel>();

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
