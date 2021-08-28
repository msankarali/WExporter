using System;

namespace Core.Entities.Concrete
{
    public abstract class BaseFullEntity : BaseStateEntity
    {
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}