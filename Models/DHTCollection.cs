using System.Collections.Generic;

namespace Jarvis_Brain.Models
{
    public class DHTCollection
    {
        public InternalComms InternalComms { get; set; }

        public ErrorLog ErrorLog { get; set; }

        public List<DHTPackage> DHTPackages { get; set; }

        public Dictionary<string, float> MaxMinTemps { get; set; }
    }
}