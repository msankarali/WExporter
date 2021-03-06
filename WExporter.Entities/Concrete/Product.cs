using Core.Entities.Concrete;

namespace WExporter.Entities.Concrete
{
    public class Product : BaseFullEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Price { get; set; }
        public string Unit { get; set; }
    }
}