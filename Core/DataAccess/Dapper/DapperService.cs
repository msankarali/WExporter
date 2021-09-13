using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Utilities.IoC;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DataAccess.Dapper
{
    public class DapperService : IDapperService
    {
        private readonly string _connectionString;

        public DapperService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int ExecuteSp<T>(string sp, object @params = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Execute(sp, @params, commandType: CommandType.StoredProcedure);
        }

        public T GetFirst<T>(string query, object @params = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(query, @params, commandType: CommandType.Text).FirstOrDefault();
        }

        public List<T> GetAll<T>(string query, object @params = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(query, @params, commandType: CommandType.Text).ToList();
        }

        public T Add<T>(string query, object @params = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var result = db.Query<T>(query, @params).FirstOrDefault();
            return result;
        }

        public T Update<T>(string query, object @params = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var result = db.Query<T>(query, @params, commandType: CommandType.Text).FirstOrDefault();
            return result;
        }

        public List<T> Query<T>(string query, object @params = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(query, @params, commandType: CommandType.Text).ToList();
        }
    }
}