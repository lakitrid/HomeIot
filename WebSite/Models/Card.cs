using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Card
    {
        public string ActionRoute { get; set; }

        public string Icon { get; set; }

        public string Label { get; set; }

        public string UnitLabel { get; set; }

        public decimal Value { get; set; }
    }
}
