using System;
using System.Collections.Generic;

namespace Jarvis_Brain.Models
{
    public partial class DhtPackages
    {
        public long DhtPackageId { get; set; }
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public long? LocationId { get; set; }
        public string Received { get; set; }

        public virtual Locations Location { get; set; }
    }
}
