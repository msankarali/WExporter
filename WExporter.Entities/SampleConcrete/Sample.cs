using System.Collections.Generic;
using Core.Entities.Concrete;

namespace WExporter.Entities.SampleConcrete
{
    public class Sample : BaseStateEntity<int>
    {
        public string Name { get; set; }
        public virtual List<Mample> Mamples { get; set; }
    }
}