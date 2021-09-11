using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.DataAccess.EntityFramework
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity>
    {
        protected BaseSpecification() => Configuration = new SpecificationConfiguration();
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria) : this() => Criteria = criteria;

        public Expression<Func<TEntity, bool>> Criteria { get; }
        public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include { get; private set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; private set; }
        public SpecificationConfiguration Configuration { get; }

        protected void SetInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFunc) => Include = includeFunc;
        protected void SetOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc) => OrderBy = orderByFunc;
    }

    class MyClass : BaseSpecification<Object>
    {
        public MyClass()
        {
            Configuration.PageSize = 30;
        }
    }
}