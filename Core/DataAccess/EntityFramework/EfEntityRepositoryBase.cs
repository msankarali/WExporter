using Core.Entities.Abstract;
using Core.Entities.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TDbContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TDbContext : DbContext
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

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

        public TResult Get<TResult>(
            Expression<Func<TEntity, TResult>> selector,
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
            var entity = (TResult)Activator.CreateInstance(typeof(TResult));
            entity = query.Select(selector).FirstOrDefault();
            return entity;
        }

        public TEntity Get(ISpecification<TEntity> specification = null)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification).FirstOrDefault();
        }

        public TResult Get<TResult>(Expression<Func<TEntity, TResult>> selector, ISpecification<TEntity> specification = null)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(selector, _dbContext.Set<TEntity>().AsQueryable(), specification).FirstOrDefault();
        }

        public IPagedList<TEntity> GetAllPaged(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification)
                .ToPagedList(
                    specification.Configuration.PageSize,
                    specification.Configuration.PageNumber);
        }

        public IPagedList<TResult> GetAllPaged<TResult>(Expression<Func<TEntity, TResult>> selector, ISpecification<TEntity> specification = null)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(selector, _dbContext.Set<TEntity>().AsQueryable(), specification)
                .ToPagedList(
                    specification.Configuration.PageSize,
                    specification.Configuration.PageNumber);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
        }

        public IPagedList<TResult> GetAllPaged<TResult>(
            Expression<Func<TEntity, TResult>> selector,
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
            if (orderBy != null) query = orderBy(query);
            return query.ToPagedList(pageSize, pageNumber, selector);
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

        public IList<TEntity> GetAll<TResult>(
            Expression<Func<TEntity, TResult>> selector,
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

        public TEntity Update(TEntity entity)
        {
            var state1 = _dbContext.Attach(entity).State;
            _dbContext.Attach(entity).State = EntityState.Modified;
            var state = _dbContext.Attach(entity).State;
            _dbContext.SaveChanges();
            return entity;
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