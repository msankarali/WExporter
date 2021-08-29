using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace WExporter.Entities.Concrete
{
    public class Group : BaseDelibleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}