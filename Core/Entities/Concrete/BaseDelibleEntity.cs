using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public abstract class BaseDelibleEntity<TId> : BaseEntity<TId>, IDelible
    {
        public virtual bool IsDeleted { get; set; } = false;
    }
}