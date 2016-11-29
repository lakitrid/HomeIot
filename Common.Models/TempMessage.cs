using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fr.Lakitrid.Common.Models
{
    public class TempMessage : Message
    {
        public string DeviceCode { get; set; }

        public decimal Temperature { get; set; }
    }
}
