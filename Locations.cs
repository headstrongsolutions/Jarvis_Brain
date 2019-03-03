using System;
using System.Collections.Generic;

namespace Jarvis_Brain.Models
{
    public partial class Locations
    {
        public Locations()
        {
            DhtPackages = new HashSet<DhtPackages>();
            InverseParentLocation = new HashSet<Locations>();
        }

        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public long? ParentLocationId { get; set; }

        public virtual Locations ParentLocation { get; set; }
        public virtual ICollection<DhtPackages> DhtPackages { get; set; }
        public virtual ICollection<Locations> InverseParentLocation { get; set; }
    }
}
