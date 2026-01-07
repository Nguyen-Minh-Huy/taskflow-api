using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Commons
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => PageIndex < TotalPages;
        public bool HasPreviousPage => PageIndex > 1;

        public PagedResult()
        {
            Items = new List<T>();
        }

        public PagedResult(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static PagedResult<T> Create(List<T> items, int count, int pageIndex, int pageSize)
        {
            return new PagedResult<T>(items, count, pageIndex, pageSize);
        }
    }
}
