using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;
using GTLNode.Services;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace GTLNode
{
    public sealed class StartupTask : IBackgroundTask
    {
        private MqttClient _client;
        private PowerService _powerService;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            this._powerService = new PowerService();

            Timer timer = new Timer(Tick, null, (int)TimeSpan.FromSeconds(5).TotalMilliseconds, (int)TimeSpan.FromSeconds(5).TotalMilliseconds);

            this._client = new MqttClient("192.168.1.220");

            this._client.Connect("GTLNode-Rpi-1");

            AutoResetEvent handle = new AutoResetEvent(false);

            handle.WaitOne();
        }

        private void Tick(object state)
        {
            string test = "test value";

            this._client.Publish("home-power", Encoding.UTF8.GetBytes(test), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }
    }
}
