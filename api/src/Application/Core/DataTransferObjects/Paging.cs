using System;

namespace DetailingArsenal.Application {
    public class Paging : IDataTransferObject {

        public int PageNumber { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int Total { get; }

        public Paging(int pageNumber, int pageSize, int total) {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = Math.Max(total / pageSize, 1);
            Total = total;
        }

        public Paging(PagingOptions options, int total) {
            PageNumber = options.PageNumber;
            PageSize = options.PageSize;
            PageCount = Math.Max(total / options.PageSize, 1);
            Total = total;
        }
    }
}