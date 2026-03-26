using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Infrastructure
{
    public class ApiResponseDto<T>
    {
        public bool status { get; set; } = false;
        public string message { get; set; }
        public T data { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount
        {
            get
            {
                if (PageNumber < TotalPages)
                    return PageSize;
                if (PageNumber > TotalPages)
                    return 0;
                return TotalCount - (PageSize * (TotalPages - 1));
            }
        }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedResponse(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items.Skip(pageSize*(pageNumber-1)).Take(pageSize).ToList();
        }
    }
}
