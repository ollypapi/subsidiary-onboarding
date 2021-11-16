using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Extentions
{
    public static class PaginationExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize) where T : class
        {
            return new PagedList<T>(query, pageIndex, pageSize);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize, CancellationToken cancellationToken = default) where T : class
        {
            var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await query.Skip((pageIndex > 0) ? (pageIndex - 1) * pageSize : 0).Take(pageSize).ToListAsync<T>(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Data = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }

        public static PagedList<TDestination> ToPagedList<TDestination, TSource>(this IMapper mapper, PagedList<TSource> source) where TSource : class where TDestination : class
        {
            var data = mapper.Map<List<TDestination>>(source.Data);

            return new PagedList<TDestination>()
            {
                Data = data,
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPages = source.TotalPages
            };
        }
    }
}
