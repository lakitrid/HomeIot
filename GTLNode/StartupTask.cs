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
        private PowerService _powerService;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            this._powerService = new PowerService();

            Timer timer = new Timer(Tick, null, (int)TimeSpan.FromSeconds(5).TotalMilliseconds, (int)TimeSpan.FromSeconds(5).TotalMilliseconds);
            
            AutoResetEvent handle = new AutoResetEvent(false);

            handle.WaitOne();
        }

        private void Tick(object state)
        {
            string test = "test value";

            
        }
    }
}
