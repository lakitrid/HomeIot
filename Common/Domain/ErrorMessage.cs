using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Domain
{
    public class ErrorMessage
    {
        public string Source { get; set; }

        public string Message { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
