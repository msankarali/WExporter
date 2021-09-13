using Core.Entities.Concrete;
using System.Collections.Generic;

namespace WExporter.Entities.Concrete
{
    public class Company : BaseFullEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<BusinessSegment> BusinessSegments { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}