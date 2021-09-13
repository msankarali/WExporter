using Core.Entities.Concrete;

namespace WExporter.Entities.Concrete
{
    public class Category : BaseActivatableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}