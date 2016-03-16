using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Message
    {
        public DateTime Date { get; set; }

        public MessageType Type { get; set; }

        public object MessageContent { get; set; }
    }
}
