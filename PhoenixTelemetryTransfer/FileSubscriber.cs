namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    public class FileSubscriber
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
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.ReadFile();
        }

        public void ReadFile()
        {
            if (File.Exists(this.path))
            {
                var inputText = File.ReadAllText(this.path);
            }
        }

        public void OnNewData()
        {
            this.NewDataArrived?.Invoke(this, EventArgs.Empty);
        }

        public bool TryGetValues(string inputText, out string[] values)
        {
            var inputArray = inputText.Split(';');

            if (inputArray.Length > 0)
            {
                this.timeStamp = inputArray[0];
                var i = 0;
                this.channelValue = new string[inputArray.Length - 1];

                foreach (var value in inputArray)
                {
                    if (i > 0)
                    {
                        this.channelValue[i - 1] = value;
                    }

                    i++;
                }
                values = this.channelValue;
                return true;
            }

            values = null;
            return false;
        }

        public int Interval => this.interval;

        public string Path => this.path;

        public string[] ChannelValue { get => this.channelValue; set => this.channelValue = value; }

        public string TimeStamp { get => this.timeStamp; set => this.timeStamp = value; }

        public bool IsStarted { get => isStarted; set => isStarted = value; }
    }
}
