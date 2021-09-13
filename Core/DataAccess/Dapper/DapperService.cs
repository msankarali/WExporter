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
            T result;
            using IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(query, @params, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Update<T>(string query, object @params = null)
        {
            T result;
            using IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(query, @params, commandType: CommandType.Text, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public List<T> Query<T>(string query, object parms = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(query, parms, commandType: CommandType.Text).ToList();
        }
    }
}