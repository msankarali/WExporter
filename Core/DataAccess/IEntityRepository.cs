using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess
{
    public interface IEntityRepository<TEntity>
        where TEntity: class, IEntity, new()
    {

    }
}