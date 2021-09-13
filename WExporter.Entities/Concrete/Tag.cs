using Core.Entities.Concrete;
using System.Collections.Generic;

namespace WExporter.Entities.Concrete
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual List<Company> Companies { get; set; }
    }
}