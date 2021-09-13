using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace WExporter.Entities.Concrete
{
    public class Group : BaseDelibleEntity<int>
    {
        public string Name { get; set; }
    }
}