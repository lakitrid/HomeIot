using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Command
    {
        public decimal Id { get; set; }

        public TaskInfo Task { get; set; }

        public string Code { get; set; }

        public decimal CommandValue { get; set; }
    }
}
