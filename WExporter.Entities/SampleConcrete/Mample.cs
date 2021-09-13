using Core.Entities.Concrete;

namespace WExporter.Entities.SampleConcrete
{
    public class Mample : BaseStateEntity<int>
    {
        public string Mame { get; set; }
        public int SampleId { get; set; }
        public virtual Sample Sample { get; set; }
    }
}