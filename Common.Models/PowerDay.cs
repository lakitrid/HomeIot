using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fr.Lakitrid.Common.Models
{
    public class PowerDay
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal PeekHourIndex { get; set; }

        public decimal LowHourIndex { get; set; }
    }
}
