using System;

namespace Jarvis_Brain.Models
{
    public class ErrorLog
    {
        public string Location { get; set; }

        public DateTime DateTime { get; set; }

        public string ExceptionMessage { get; set; }
    }
}