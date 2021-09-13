using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Abstract;
using Core.Entities.PagedList;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.DataAccess.EntityFramework
{
    public interface IEntityRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        TEntity GetFirst(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);
        TResult GetSelect<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);

        TEntity GetSpec(ISpecification<TEntity> specification = null);
        TResult GetSelectSpec<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            ISpecification<TEntity> specification = null);

        IPagedList<TEntity> GetAllPagedSpec(ISpecification<TEntity> specification);
        IPagedList<TResult> GetAllPagedSelectSpec<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            ISpecification<TEntity> specification = null);

        IPagedList<TResult> GetAllPaged<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false,
            int pageNumber = 0,
            int pageSize = 20
            );

        IPagedList<TEntity> GetAllPaged(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false,
            int pageNumber = 0,
            int pageSize = 20
            );

        IReadOnlyList<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false
            );

        IReadOnlyList<TEntity> GetAllSelect<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false
            );

        TEntity Add(TEntity entity);
        int Add(params TEntity[] entities);
        int Add(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        int Update(params TEntity[] entities);
        int Update(IEnumerable<TEntity> entities);

        TEntity Delete(TEntity entity);
        int Delete(params TEntity[] entities);
        int Delete(params object[] keyValues);
        int Delete(IEnumerable<TEntity> entities);

        int Count(
            Expression<Func<TEntity, bool>> predicate = null,
            bool ignoreQueryFilters = false);

        bool Any(
            Expression<Func<TEntity, bool>> predicate,
            bool ignoreQueryFilters = false);
    }
}