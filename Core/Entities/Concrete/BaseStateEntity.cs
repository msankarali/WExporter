using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public abstract class BaseStateEntity<TId> : BaseEntity<TId>, IDelible, IActivatable
    {
        public virtual bool IsActive { get; set; } = true;
        public virtual bool IsDeleted { get; set; } = false;
    }
}