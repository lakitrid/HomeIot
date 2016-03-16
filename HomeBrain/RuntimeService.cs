using HomeBrain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain
{
    partial class RuntimeService : ServiceBase
    {
        public RuntimeService()
        {
            InitializeComponent();
        }

#if DEBUG
        public void Run()
        {
            this.OnStart(null);

            this.WaitForInput();

            this.OnStop();
        }

        private void WaitForInput()
        {
            string input = string.Empty;

            do
            {
                Console.WriteLine("Type <stop> to end the debug session");
                input = Console.ReadLine();
            } while (!input.Equals("stop"));
        }
#endif
        protected override void OnStart(string[] args)
        {
            ScheduleService.Instance.Start();
        }

        protected override void OnStop()
        {
            ScheduleService.Instance.Stop();
        }
    }
}
