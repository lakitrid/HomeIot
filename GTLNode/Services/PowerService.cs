using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace GTLNode.Services
{
    internal class PowerService
    {
        public PowerService()
        {
            this.Init();
        }

        public AutoResetEvent Handle { get; private set; }

        private async void Init()
        {
            this.Handle = new AutoResetEvent(false);

        }

        private void Run()
        {

        }

        private async Task<bool> InitSerial()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector("UART0");
                DeviceInformationCollection dis = await DeviceInformation.FindAllAsync(aqs);
                SerialDevice serialPort = await SerialDevice.FromIdAsync(dis[0].Id);

                serialPort.BaudRate = 1200;
                serialPort.Parity = SerialParity.Even;
                serialPort.DataBits = 7;
                serialPort.StopBits = SerialStopBitCount.One;

                serialPort.ReadTimeout = TimeSpan.FromSeconds(1);

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
