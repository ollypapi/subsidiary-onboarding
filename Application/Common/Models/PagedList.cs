using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Common.Models
{
    public class PagedList<T> where T : class
    {
        public PagedList()
        {
        }

        internal PagedList(IQueryable<T> query, int page, int pageSize)
        {
            CurrentPage = page;
            PageSize = pageSize;
            TotalCount = query.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
            Data = query.Skip((page > 0) ? (page - 1) * pageSize : 0).Take(pageSize).ToList();
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> Data { get; set; }
    }
}
