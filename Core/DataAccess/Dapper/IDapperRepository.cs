using System.Collections.Generic;
using Core.Entities.Abstract;
using Core.Entities.PagedList;

namespace Core.DataAccess.Dapper
{
    public interface IDapperRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        TEntity GetFirstById(TId id);
        IReadOnlyList<TEntity> GetAll();
        /// <param name="orderColumnName">Use carefully!</param>
        IPagedList<TEntity> GetAllPaged(int pageNumber = 0, int pageSize = 20, string orderColumnName = "id", bool descending = true);
        int Delete(TEntity entity);
        int DeleteById(TId id);
        int Query(string query, object @params);
    }
}