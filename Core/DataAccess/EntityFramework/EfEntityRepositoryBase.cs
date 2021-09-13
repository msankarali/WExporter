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
    public class EfEntityRepositoryBase<TEntity, TId> : IEntityRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public EfEntityRepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public TEntity GetFirst(
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

        public TResult GetSelect<TResult>(
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

        public TEntity GetSpec(ISpecification<TEntity> specification = null)
        {
            return SpecificationEvaluator<TEntity, TId>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification).FirstOrDefault();
        }

        public TResult GetSelectSpec<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            ISpecification<TEntity> specification = null)
        {
            return SpecificationEvaluator<TEntity, TId>.GetQuery(selector, _dbContext.Set<TEntity>().AsQueryable(), specification).FirstOrDefault();
        }

        public IPagedList<TEntity> GetAllPagedSpec(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TId>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification)
                .ToPagedList(
                    specification.Configuration.PageSize,
                    specification.Configuration.PageNumber);
        }

        public IPagedList<TResult> GetAllPagedSelectSpec<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            ISpecification<TEntity> specification = null)
        {
            return SpecificationEvaluator<TEntity, TId>.GetQuery(selector, _dbContext.Set<TEntity>().AsQueryable(), specification)
                .ToPagedList(
                    specification.Configuration.PageSize,
                    specification.Configuration.PageNumber);
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

        public IReadOnlyList<TEntity> GetAll(
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

        public IReadOnlyList<TEntity> GetAllSelect<TResult>(
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
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
            return entity;
        }

        public int Delete(params TEntity[] entities)
        {

            _dbContext.Entry(entities).State = EntityState.Deleted;
            return _dbContext.SaveChanges();
        }

        public int Delete(IEnumerable<TEntity> entities)
        {
            _dbContext.Entry(entities).State = EntityState.Deleted;
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
            _dbContext.Entry(entity).State = EntityState.Added;
            _dbContext.SaveChanges();
            return entity;
        }

        public int Add(params TEntity[] entities)
        {
            _dbContext.Entry(entities).State = EntityState.Added;
            return _dbContext.SaveChanges();
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            _dbContext.Entry(entities).State = EntityState.Added;
            return _dbContext.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
         }

        public int Update(params TEntity[] entities)
        {
            _dbContext.Entry(entities).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            _dbContext.Entry(entities).State = EntityState.Modified;
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