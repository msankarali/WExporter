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
    public class EfEntityRepositoryBase<TEntity, TDbContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TDbContext : DbContext, new()
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
            var entityToDelete = _dbSet.Remove(entity).Entity;
            _dbContext.SaveChanges();
            return entityToDelete;
        }

        public int Delete(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChanges();
        }

        public int Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChanges();
        }

        public int Delete(params object[] keyValues)
        {
            TEntity entityToDelete = _dbSet.Find(keyValues);
            _dbSet.Remove(entityToDelete);
            return _dbContext.SaveChanges();
        }

        public TEntity Add(TEntity entity)
        {
            var entityToAdd = _dbSet.Add(entity).Entity;
            _dbContext.SaveChanges();
            return entityToAdd;
        }

        public int Add(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public (TEntity old, TEntity @new) Update(TEntity entity)
        {
            var entityToUpdate = _dbSet.Update(entity).Entity;
            _dbContext.SaveChanges();
            return (entity, entityToUpdate);
        }

        public int Update(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
            return _dbContext.SaveChanges();
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            return _dbContext.SaveChanges();
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