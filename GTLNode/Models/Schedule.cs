using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTLNode.Models
{
    public sealed class Schedule
    {
        public string Name { get; set; }

        public int Interval { get; set; }

        public int StartDelay { get; set; }
    }
}
