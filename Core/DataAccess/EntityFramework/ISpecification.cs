using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.DataAccess.EntityFramework
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include { get; }
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; }
        SpecificationConfiguration Configuration { get; }
    }
}