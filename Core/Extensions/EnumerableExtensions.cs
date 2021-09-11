﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            return (condition
                ? source.Where(predicate)
                : source) as IQueryable<TSource>;
        }
    }
}