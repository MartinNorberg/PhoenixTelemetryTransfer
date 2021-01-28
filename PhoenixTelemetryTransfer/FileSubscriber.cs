// <copyright file="FileSubscriber.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PhoenixTelemetryTransfer
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;

    public class FileSubscriber : INotifyPropertyChanged
    {
        private readonly int interval;
        private readonly string path;
        private string[] channelValue;
        private string timeStamp;
        private bool isStarted;
        private DispatcherTimer dispatcherTimer;

        public FileSubscriber(string path, int interval)
        {
            if (string.IsNullOrEmpty(path) || interval <= 0)
            {
                throw new ArgumentException("Please put a value in path and interval");
            }

            this.path = path;
            this.interval = interval;
        }

        public event EventHandler NewDataArrived;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Interval => this.interval;

        public string[] ChannelValue
        {
            get => this.channelValue;

            set
            {
                if (Equals(this.channelValue, value))
                {
                    return;
                }

                this.channelValue = value;
                this.OnPropertyChanged();
            }
        }

        public string TimeStamp
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

        public bool IsStarted
        {
            get => this.isStarted;

            set
            {
                if (Equals(this.isStarted, value))
                {
                    return;
                }

                this.isStarted = value;
                this.OnPropertyChanged();
            }
        }

        public string Path => this.path;

        public void Start()
        {
            if (this.isStarted)
            {
                throw new ApplicationException("Subscriber is already started");
            }

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += this.DispatcherTimer_Tick;
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, this.interval);
            this.dispatcherTimer.Start();
            this.isStarted = true;
            this.OnPropertyChanged(nameof(this.IsStarted));
        }

        public void Stop()
        {
            if (!this.isStarted)
            {
                throw new ApplicationException("Subscriber is already stopped");
            }

            this.dispatcherTimer.Stop();
            this.dispatcherTimer.Tick -= this.DispatcherTimer_Tick;
            this.dispatcherTimer = null;
            this.isStarted = false;
            this.OnPropertyChanged(nameof(this.IsStarted));
        }

        public void ReadFile()
        {
            if (File.Exists(this.path))
            {
                var inputText = File.ReadAllText(this.path);
                this.TryGetValues(inputText, out string[] values);
            }
        }

        public void OnNewData()
        {
            this.NewDataArrived?.Invoke(this, EventArgs.Empty);
        }

        // Format of textfile from logger is: "00:00:05, 21.1, 22.2, 21.3"  comma , separeted
        //                                   timestamp, ch1,   ch2,  ch3
        public bool TryGetValues(string inputText, out string[] values)
        {
            var inputArray = inputText.Split(',');

            if (inputArray.Length > 0)
            {
                this.TimeStamp = inputArray[0];
                var i = 0;
                this.ChannelValue = new string[inputArray.Length - 1];

                foreach (var value in inputArray)
                {
                    if (i > 0)
                    {
                        this.channelValue[i - 1] = value;
                    }

                    i++;
                }

                values = this.channelValue;
                this.OnNewData();
                return true;
            }

            values = null;
            return false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.ReadFile();
        }
    }
}
