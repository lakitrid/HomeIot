using Common.Domain;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private SerialDevice _serialPort;
        private readonly TimeSpan period;
        private readonly TimeSpan start;
        private Timer _timer;
        private object readLock = new object();

        public PowerService()
        {
            this.period = TimeSpan.FromSeconds(60);
            this.start = TimeSpan.FromSeconds(5);

            this.Init();
        }

        public AutoResetEvent Handle { get; private set; }

        public void Stop()
        {
            this._timer.Dispose();
        }

        /// <summary>
        /// Initialaze the running task
        /// </summary>
        private void Init()
        {
            this.Handle = new AutoResetEvent(false);
            Debug.WriteLine("Init power service");

            this._timer = new Timer(this.Run, null, this.start, this.period);
        }

        /// <summary>
        /// Running Task that will read the Serial port at time interval
        /// 
        /// Read the TeleInfo frame
        /// 
        /// Decode TeleInfo Data Sample frame :
        /// Start with STX (0x02)
        /// Serial number :  ADCO SerialNumber C
        /// Type of billing : OPTARIF HC.. <
        /// Level of power : ISOUSC 30 9
        /// Low Hour (in WH) : HCHC 034172198 )
        /// Peek Hour (IN WH) : HCHP 036245714 3
        /// current period : PTEC HP..  
        /// Instant intensity (in A) : IINST 003 Z
        /// Max intensoty (in A) : IMAX 029 J
        /// Apparent Power (in VA) : PAPP 00820 +
        /// Warning over intensity ( in A) : ADPS
        /// HHPHC A ,
        /// MOTDETAT 000000 B
        /// End with ETX (0x03)
        /// </summary>
        /// <param name="state">object state</param>
        private async void Run(object state)
        {
            Debug.WriteLine("Start reading method");

            try
            {
                bool result = await this.Read();
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"Error while reading : {exc.Message}");
            }
        }

        private async Task<bool> Read()
        {
            if (await this.InitSerial())
            {
                bool keep = false;
                List<string> keepLines = new List<string>();
                string[] lines = await this.ReadLines();

                if (lines != null)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("ADCO"))
                        {
                            if (!keep)
                            {
                                keep = true;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (keep)
                        {
                            keepLines.Add(lines[i]);
                        }
                    }
                }

                if (keepLines.Count > 0)
                {
                    Debug.WriteLine("Start Decoding loop");

                    // Here we are at the start of a frame, we read each lines until the next frame
                    TeleInfoData infoData = new TeleInfoData() { Date = DateTime.Now };

                    foreach (string keepLine in keepLines)
                    {
                        string[] datas = keepLine.Split(new char[] { ' ' });

                        switch (datas[0])
                        {
                            case "ADCO":
                                infoData.MeterId = datas[1];
                                break;
                            case "ISOUSC":
                                infoData.AccountIntensity = int.Parse(datas[1]);
                                break;
                            case "HCHC":
                                infoData.LowHourCpt = decimal.Parse(datas[1]);
                                break;
                            case "HCHP":
                                infoData.PeekHourCpt = decimal.Parse(datas[1]);
                                break;
                            case "IINST":
                                infoData.ActualIntensity = int.Parse(datas[1]);
                                break;
                            case "IMAX":
                                infoData.MaxIntensity = int.Parse(datas[1]);
                                break;
                            case "PAPP":
                                infoData.ApparentPower = int.Parse(datas[1]);
                                break;
                            case "ADPS":
                                infoData.HasExceed = true;
                                break;
                        }
                    }

                    Debug.WriteLine($"End Decoding loop {SerializeHelper.Serialize(infoData)}");

                    MQTTService.Instance.PublishMessage(MQTTService.PowerRoutingKey, SerializeHelper.Serialize(infoData));
                }

                this.CloseSerial();
            }

            return true;
        }

        private async Task<string[]> ReadLines()
        {
            Debug.WriteLine("Start ReadLines method");

            try
            {
                string rxData = string.Empty;

                do
                {
                    Debug.WriteLine("Read");

                    const uint maxReadLength = 256;
                    DataReader dataReader = new DataReader(this._serialPort.InputStream);
                    uint bytesToRead = await dataReader.LoadAsync(maxReadLength);
                    byte[] buffer = new byte[bytesToRead];
                    dataReader.ReadBytes(buffer);
                    string text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    rxData += text;

                    Debug.WriteLine("End Read");
                }
                while (rxData.IndexOf("ADCO") == rxData.LastIndexOf("ADCO"));

                return rxData.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception exc)
            {
                ErrorMessage error = new ErrorMessage
                {
                    Source = nameof(PowerService),
                    Message = "Error while reading serial port",
                    ExceptionMessage = exc.Message
                };

                string message = SerializeHelper.Serialize(error);

                MQTTService.Instance.PublishMessage(MQTTService.PowerErrorRoutingKey, message);
            }

            Debug.WriteLine("End ReadLines method");

            return null;
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
            Debug.WriteLine("Start InitSerial method");

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

                Debug.WriteLine("End InitSerial method with success");

                return true;
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"Exception in InitSerial method {exc.Message}");

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
