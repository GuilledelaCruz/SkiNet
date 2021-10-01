using API.DTOs;
using System.Collections.Generic;

namespace API.Helpers
{
    public class Pagination<T> where T : class
    {
        public int PageIndex{ get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageIndex, int pageSize, int number, IReadOnlyList<T> map)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = number;
            Data = map;
        }
    }
}