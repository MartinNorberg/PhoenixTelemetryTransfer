// <copyright file="ViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Ookii.Dialogs.Wpf;

    /// <summary>
    /// ViewModel for mainWindow.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private string path;
        private ObservableCollection<Channel> channels;
        private int selectedNoChannels;
        private int[] noChannels;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.channels = new ObservableCollection<Channel>();
            this.noChannels = new int[20] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            this.TryCatch(() => this.LoadConfig());
            this.BrowseCommand = new RelayCommand(_ => this.TryCatch(() => this.BrowsePath()));
            this.OnPropertyChanged(nameof(this.NoChannels));
        }

        /// <summary>
        /// EventHandler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the number of channels to be used.
        /// </summary>
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

        /// <summary>
        /// Gets or sets Path to file to be subscribed.
        /// </summary>
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

        /// <summary>
        /// Gets Command for browsing path to file that filesubscriber uses.
        /// </summary>
        public ICommand BrowseCommand { get; }

        /// <summary>
        /// Gets command for starting subscribing and writing to PLC.
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Gets Exception list.
        /// </summary>
        public ObservableCollection<Exception> Exceptions { get; } = new ObservableCollection<Exception>();

        /// <summary>
        /// Gets or sets a list of channels.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the selected number of channels to be used.
        /// </summary>
        public int SelectedNoChannels
        {
            get => this.selectedNoChannels;
            set
            {
                if (Equals(this.selectedNoChannels, value))
                {
                    return;
                }

                this.selectedNoChannels = value;
                this.OnPropertyChanged();

                // Ugly side effect...
                this.SelectedNoChannelsChanged();
            }
        }

        /// <summary>
        /// Wrapped TryCatch method and log exceptions to Exceptions list.
        /// </summary>
        /// <param name="action">Action to execute.</param>
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

        /// <summary>
        /// Signal to gui that property has changed.
        /// </summary>
        /// <param name="propertyName">Optional, nameof property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SelectedNoChannelsChanged()
        {
            this.UpdateSetting("NoChannels", this.selectedNoChannels.ToString());
            var noChannels = this.channels.Count;

            if (noChannels < this.selectedNoChannels)
            {
                for (int i = noChannels + 1; i <= this.selectedNoChannels; i++)
                {
                    var newChannel = new Channel();
                    newChannel.ChannelName = $"Channel{i}";
                    this.channels.Add(newChannel);
                }
            }
            else if (this.channels.Count > this.selectedNoChannels)
            {
                var channelsToRemove = this.channels.Count - this.selectedNoChannels;

                for (int i = 1; i <= channelsToRemove; i++)
                {
                    this.channels.RemoveAt(this.channels.Count - 1);
                }
            }

            this.LoadConfig();
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

            for (int i = 1; i <= this.SelectedNoChannels; i++)
            {
                var channelName = $"Channel{i}";
                var name = ConfigurationManager.AppSettings[$"Channel{i}Name"].ToString();
                var tag = ConfigurationManager.AppSettings[$"Channel{i}OpcTag"].ToString();

                foreach (var item in this.channels)
                {
                    if (item.ChannelName == channelName)
                    {
                        item.Name = name;
                        item.Tag = tag;
                    }
                }
            }
        }

        private void UpdateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
