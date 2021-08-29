using Core.Entities.Concrete;

namespace WExporter.Entities.Concrete
{
    public class Category : BaseActivatableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}