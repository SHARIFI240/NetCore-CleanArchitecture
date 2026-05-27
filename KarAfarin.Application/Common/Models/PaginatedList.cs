using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PaginatedList(List<T> items, int count, int page)
        {
            PageNumber = page;
            TotalPages = (int)Math.Ceiling(count / (double)20);
            TotalCount = count;
            Items = items;
        }
    }
}
