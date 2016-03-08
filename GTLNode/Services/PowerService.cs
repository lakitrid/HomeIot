using Common.Domain;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace GTLNode.Services
{
    internal class PowerService
    {
        private Task _task;
        private CancellationTokenSource _tokenSource;
        private SerialDevice _serialPort;
        private readonly TimeSpan period;
        private readonly TimeSpan start;
        private Timer _timer;

        public PowerService()
        {
            this.period = TimeSpan.FromMinutes(2);
            this.start = TimeSpan.FromSeconds(5);

            this.Init();
        }

        public AutoResetEvent Handle { get; private set; }

        public void Stop()
        {
            this._tokenSource.Cancel();
            this._task.Wait();
        }

        /// <summary>
        /// Initialaze the running task
        /// </summary>
        private void Init()
        {
            this.Handle = new AutoResetEvent(false);

            this._tokenSource = new CancellationTokenSource();

            this._timer = new Timer(this.Run, null, this.start, this.period);
        }

        /// <summary>
        /// Running Task that will read the Serial port at time interval
        /// </summary>
        /// <param name="state">object state</param>
        private async void Run(object state)
        {
            if (await this.InitSerial())
            {
                const uint maxReadLength = 1024;
                DataReader dataReader = new DataReader(this._serialPort.InputStream);
                uint bytesToRead = await dataReader.LoadAsync(maxReadLength);
                string rxBuffer = dataReader.ReadString(bytesToRead);

                this.CloseSerial();
            }
        }

        /// <summary>
        /// Close the serial port
        /// </summary>
        private void CloseSerial()
        {
            this._serialPort.Dispose();
        }

        private async Task<bool> InitSerial()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector("UART0");
                DeviceInformationCollection dis = await DeviceInformation.FindAllAsync(aqs);
                this._serialPort = await SerialDevice.FromIdAsync(dis[0].Id);

                this._serialPort.BaudRate = 1200;
                this._serialPort.Parity = SerialParity.Even;
                this._serialPort.DataBits = 7;
                this._serialPort.StopBits = SerialStopBitCount.One;

                this._serialPort.ReadTimeout = TimeSpan.FromSeconds(1);

                return true;
            }
            catch (Exception exc)
            {
                ErrorMessage error = new ErrorMessage
                {
                    Source = nameof(PowerService),
                    Message = "Error while openning serial port",
                    ExceptionMessage = exc.Message
                };

                string message = SerializeHelper.Serialize(error);

                MQTTService.Instance.PublishMessage(MQTTService.PowerErrorRoutingKey, message);

                return false;
            }
        }
    }
}
