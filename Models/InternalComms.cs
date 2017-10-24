using System;
using Jarvis_Brain.Code;

namespace Jarvis_Brain.Models
{
    public class InternalComms
    {
        public string Name { get; set; }

        public DateTime? Received { get; set; }

        public DateTime? Sent { get; set; }

        public StateEnum State { get; set; }

        public ErrorType? ErrorType { get; set; }

        public string Message { get; set; }
    }
}
