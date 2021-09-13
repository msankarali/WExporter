using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace WExporter.Entities.Concrete
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}