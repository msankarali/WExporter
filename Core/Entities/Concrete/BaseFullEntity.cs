using System;

namespace Core.Entities.Concrete
{
    public abstract class BaseFullEntity<TId> : BaseStateEntity<TId>
    {
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime? ModifiedDate { get; set; }
    }
}