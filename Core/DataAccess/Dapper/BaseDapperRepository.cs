using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Entities.Abstract;
using Core.Entities.PagedList;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Core.DataAccess.Dapper
{
    public abstract class BaseDapperRepository<TEntity, TId> : IDapperRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        protected BaseDapperRepository(IConfiguration configuration, string tableName)
        {
            _tableName = tableName;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection SqlConnection => new SqlConnection(_connectionString);

        public TEntity GetFirstById(TId id)
        {
            using IDbConnection connection = SqlConnection;
            var result = connection.QueryFirstOrDefault<TEntity>(
                $"select * from {_tableName} where Id=@Id", new { Id = id });
            return result;
        }

        public IReadOnlyList<TEntity> GetAll()
        {
            using IDbConnection connection = SqlConnection;
            var result = connection.Query<TEntity>($"select * from {_tableName}");
            return result as IReadOnlyList<TEntity>;
        }

        public IPagedList<TEntity> GetAllPaged(int pageNumber = 0, int pageSize = 20, string orderColumnName = "Id", bool descending = true)
        {
            using IDbConnection connection = SqlConnection;
            int totalCount = connection.ExecuteScalar<int>($"select count(*) from {_tableName}");
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            orderColumnName = orderColumnName.Replace('\'', ' ').Replace('"', ' ').Replace('=', ' ');
            var result = connection.Query<TEntity>(
                $"select * from {_tableName} order by {orderColumnName} {(descending ? "desc" : "")} offset @offset rows fetch next @next rows only",
                new
                {
                    offset = pageNumber * pageSize,
                    next = pageSize,
                });
            return result.ToPagedList(pageSize, pageNumber, totalCount, totalPages);
        }

        public int Delete(TEntity entity)
        {
            using IDbConnection connection = SqlConnection;
            var result = connection.Execute($"delete from {_tableName} where Id = @Id", new { Id = entity.Id });
            return result;
        }

        public int DeleteById(TId id)
        {
            using IDbConnection connection = SqlConnection;
            var result = connection.Execute($"delete from {_tableName} where Id = @Id", new { Id = id });
            return result;
        }

        public int Query(string query, object @params)
        {
            using IDbConnection connection = SqlConnection;
            var result = connection.Execute(query, @params);
            return result;
        }
    }
}