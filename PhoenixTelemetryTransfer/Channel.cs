namespace PhoenixTelemetryTransfer
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Runtime.CompilerServices;
    using System.Windows;

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
                //Måste finnas bättre sätt än detta....
                this.OnPropertyChanged();
                UpdateSetting(channelName + "OpcTag", value);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
