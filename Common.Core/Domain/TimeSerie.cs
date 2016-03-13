using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class TimeSerie
    {
        public decimal Id { get; set; }

        public DateTime Date { get; set; }

        public string Code { get; set; }

        public decimal Value { get; set; }
    }
}
