using System;
using System.Collections.Generic;
using Dapper;

namespace Core.DataAccess.Dapper
{
    public interface IDapperService
    {
        T GetFirst<T>(string query, object @params = null);

        List<T> GetAll<T>(string query, object @params = null);

        T Add<T>(string query, object @params = null);

        T Update<T>(string query, object @params = null);

        int ExecuteSp<T>(string sp, object @params = null);

        List<T> Query<T>(string query, object @params = null);
    }
}