using System;

namespace Jarvis_Brain.Models
{
    public class DHTPackage
    {
        public float Temperature { get; set; }

        public float Humidity { get; set; }

        public float BatteryLevel { get; set; }

        public string Location { get; set; }

        public DateTime Received { get; set; }
    }
}
