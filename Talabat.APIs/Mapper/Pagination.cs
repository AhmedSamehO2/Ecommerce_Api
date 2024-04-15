using Talabat.APIs.DTOS;

namespace Talabat.APIs.Mapper
{
    public class Pagination<T>
    {
       

        public Pagination(int pageIndex, int pageSize, IEnumerable<T> data, int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
