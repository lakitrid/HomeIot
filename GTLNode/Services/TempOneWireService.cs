using GTLNode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTLNode.Services
{
    internal sealed class TempOneWireService
    {
        private static readonly TempOneWireService instance = new TempOneWireService();

        private TempOneWireService()
        {
            Schedule schedule = new Schedule
            {
                Name = "TempOneWire",
                Interval = 120,
                StartDelay = 20
            };

            SchedulerService.Instance.RegisterAction(this.Run, schedule);
        }

        public static TempOneWireService Instance { get { return instance; } }

        public void Run()
        {

        }
    }
}
