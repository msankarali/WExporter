using System;

namespace Core.Entities.Abstract
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}