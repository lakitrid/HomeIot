using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTLNode.Models
{
    internal class ScheduleRunner
    {
        public Schedule Schedule { get; set; }

        public int PeriodsBeforeNextRun { get; set; }

        public Action Action { get; set; }
        public bool IsRunning { get; internal set; }
    }
}
