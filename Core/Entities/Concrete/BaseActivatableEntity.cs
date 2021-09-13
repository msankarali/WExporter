using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public abstract class BaseActivatableEntity<TId> : BaseEntity<TId>, IActivatable
    {
        public virtual bool IsActive { get; set; } = true;
    }
}
