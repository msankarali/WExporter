using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework
{
    internal class SpecificationEvaluator<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        public static SpecificationEvaluator<TEntity, TId> CreateInstance()
        {
            return new SpecificationEvaluator<TEntity, TId>();
        }

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            if (!specification.Configuration.EnableTracking) query = query.AsNoTracking();
            if (specification.Include != null) query = specification.Include(query);
            if (specification.Criteria != null) query = query.Where(specification.Criteria);
            if (specification.Configuration.IgnoreQueryFilters) query = query.IgnoreQueryFilters();
            if (specification.OrderBy != null) query = specification.OrderBy(query);
            return query;
        }

        public static IQueryable<TResult> GetQuery<TResult>(Expression<Func<TEntity, TResult>> selector, IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            if (!specification.Configuration.EnableTracking) query = query.AsNoTracking();
            if (specification.Include != null) query = specification.Include(query);
            if (specification.Criteria != null) query = query.Where(specification.Criteria);
            if (specification.Configuration.IgnoreQueryFilters) query = query.IgnoreQueryFilters();
            if (specification.OrderBy != null) query = specification.OrderBy(query);
            return query.Select(selector);
        }
    }
}