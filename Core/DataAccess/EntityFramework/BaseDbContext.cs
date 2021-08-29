using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var key in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                key.DeleteBehavior = DeleteBehavior.Restrict;
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IActivatable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression<IActivatable>(e => e.IsActive, entityType.ClrType));
                }

                if (typeof(IDelible).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression<IDelible>(e => !e.IsDeleted, entityType.ClrType));
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        private static LambdaExpression ConvertFilterExpression<TInterface>(
            Expression<Func<TInterface, bool>> filterExpression,
            Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }
}