using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public abstract class BaseDelibleEntity : BaseEntity, IDelible
    {
        public virtual bool IsDeleted { get; set; } = false;
    }
}