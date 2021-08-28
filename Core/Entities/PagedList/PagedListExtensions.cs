using System;
using System.Linq;

namespace Core.Entities.PagedList
{
    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageSize, int pageNumber = 0)
        {
            int count = source.Count();
            int totalPages = (int)Math.Ceiling((double)count / pageSize);
            if (totalPages == 0) totalPages = 1;
            if (pageNumber >= totalPages) throw new Exception("404 ^__^");
            var pagedList = source.Skip(pageNumber * pageSize).Take(pageSize);
            return new PagedList<T>(pageNumber, pageSize, count, totalPages, pagedList.ToList());
        }
    }
}