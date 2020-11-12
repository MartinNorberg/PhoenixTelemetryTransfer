// <copyright file="Channel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PhoenixTelemetryTransfer
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    public class Channel : INotifyPropertyChanged
    {
        private string channelName;
        private string name;
        private string value;
        private DateTime timeStamp;
        private string tag;

        public Channel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ChannelName
        {
            get => this.channelName;

            set
            {
                if (Equals(this.channelName, value))
                {
                    return;
                }

                this.channelName = value;
                this.OnPropertyChanged();
            }
        }

        public string Name
        {
            get => this.name;

            set
            {
                if (Equals(this.channelName, value))
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
                this.UpdateSetting($"{this.channelName}Name", value);
            }
        }

        public string Value
        {
            get => this.value;

            set
            {
                if (Equals(this.value, value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime TimeStamp
        {
            get => this.timeStamp;

            set
            {
                if (Equals(this.timeStamp, value))
                {
                    return;
                }

                this.timeStamp = value;
                this.OnPropertyChanged();
            }
        }

        public string Tag
        {
            get => this.tag;

            set
            {
                if (Equals(this.tag, value))
                {
                    return;
                }

                this.tag = value;
                this.OnPropertyChanged();
                this.UpdateSetting($"{this.channelName}OpcTag", value);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
