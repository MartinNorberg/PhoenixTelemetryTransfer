namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Ookii.Dialogs.Wpf;
    using System.Windows.Input;
    using System.Configuration;
    using System.Windows;
    using System.Collections.Generic;

    public class ViewModel: INotifyPropertyChanged
    {
        private string path;
        private ObservableCollection<Channel> channels;
        private int selectedNoChannel;
        private int[] noChannels;

        public int[] NoChannels
        {
            get => this.noChannels;
            set
            {
                if (ReferenceEquals(value, this.noChannels))
                {
                    return;
                }

                this.noChannels = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            this.channels = new ObservableCollection<Channel>();
            this.noChannels = new int[20] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            this.TryCatch(() => this.LoadConfig());
            this.BrowseCommand = new RelayCommand(_ => this.TryCatch(() => this.BrowsePath()));
            this.OnPropertyChanged(nameof(this.NoChannels));
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
            if (int.TryParse(ConfigurationManager.AppSettings["NoChannels"].ToString(), out var savedNoChannels))
            {
                this.SelectedNoChannels = savedNoChannels;
            }
            else
            {
                this.SelectedNoChannels = 0;
            }
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

        public ObservableCollection<Channel> Channels
        {
            get => this.channels;
            set
            {
                if (ReferenceEquals(value, this.channels))
                {
                    return;
                }

                this.channels = value;
                this.OnPropertyChanged();
            }
        }

        public int SelectedNoChannels 
        { 
            get => this.selectedNoChannel;
            set
            {
                if (Equals(this.selectedNoChannel, value))
                {
                    return;
                }
                
                this.selectedNoChannel = value;
                this.OnPropertyChanged();

                // Ugly side effect...
                this.SelectedNoChannelsChanged();
            }
        }

        private void SelectedNoChannelsChanged()
        {
            this.UpdateSetting("NoChannels", this.selectedNoChannel.ToString());
            if (this.channels.Count == 0)
            {
                for (int i = 1; i <= this.selectedNoChannel; i++)
                {
                    var newChannel = new Channel();
                    newChannel.ChannelName = $"Channel{i}";
                    this.channels.Add(newChannel);
                }
            }
            else if (this.channels.Count > this.selectedNoChannel)
            {
                var channelsToRemove = this.channels.Count - this.selectedNoChannel;

                for (int i = 1; i <= channelsToRemove; i++)
                {
                    this.channels.RemoveAt(this.channels.Count-1);
                }
            }
        }

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
