using Core.Entities.Abstract;
using Core.Entities.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public readonly DbContext _dbContext;
        public readonly DbSet<TEntity> _dbSet;

        public EfEntityRepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public TEntity Get(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool ignoreQueryFilters = false,
            bool enableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return query.FirstOrDefault();
        }

        public TEntity Get<TResult>(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, TResult>> selector = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool ignoreQueryFilters = false,
            bool enableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (selector != null) query = (IQueryable<TEntity>)query.Select(selector).AsQueryable();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return query.FirstOrDefault();
        }

        public IPagedList<TEntity> GetAllPaged<TResult>(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, TResult>> selector = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool ignoreQueryFilters = false,
            bool enableTracking = true,
            int pageNumber = 0,
            int pageSize = 20)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (selector != null) query = (IQueryable<TEntity>)query.Select(selector).AsQueryable();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return orderBy != null
                ? orderBy(query).ToPagedList(pageSize, pageNumber)
                : query.ToPagedList(pageSize, pageNumber);
        }

        public IPagedList<TEntity> GetAllPaged(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false,
            int pageNumber = 0,
            int pageSize = 20)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return orderBy != null
                ? orderBy(query).ToPagedList(pageSize, pageNumber)
                : query.ToPagedList(pageSize, pageNumber);
        }

        public IList<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return orderBy != null
                ? orderBy(query).ToList()
                : query.ToList();
        }

        public IList<TEntity> GetAll<TResult>(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, TResult>> selector = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (selector != null) query = (IQueryable<TEntity>)query.Select(selector).AsQueryable();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return orderBy != null
                ? orderBy(query).ToList()
                : query.ToList();
        }

        public TEntity Delete(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

        public void Delete(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(params object[] keyValues)
        {
            TEntity entityToDelete = _dbSet.Find(keyValues);
            _dbSet.Remove(entityToDelete);
        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void Add(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public (TEntity old, TEntity @new) Update(TEntity entity)
        {
            return (entity, _dbSet.Update(entity).Entity);
        }

        public void Update(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public (TEntity old, TEntity @new) Update(TEntity entity, params object[] keyValues)
        {
            var entityToUpdate = _dbSet.Find(keyValues);
            if (entityToUpdate != null)
            {
                return (entity, _dbSet.Update(entity).Entity);
            }
            return default;
        }

        public int Count(
            Expression<Func<TEntity, bool>> predicate = null,
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return predicate == null
                ? query.Count()
                : query.Count(predicate);
        }

        public bool Any(
            Expression<Func<TEntity, bool>> predicate,
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return predicate == null
                ? query.Any()
                : query.Any(predicate);
        }
    }
}