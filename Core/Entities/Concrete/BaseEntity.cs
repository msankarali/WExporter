using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
    }
}