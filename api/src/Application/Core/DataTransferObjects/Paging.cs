namespace DetailingArsenal.Application {
    public class Paging : IDataTransferObject {

        public int PageNumber { get; }
        public int PageSize { get; }
        public int Total { get; }

        public Paging(int pageNumber, int pageSize, int total) {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Total = total;
        }

        public Paging(PagingOptions options, int total) {
            PageNumber = options.PageNumber;
            PageSize = options.PageSize;
            Total = total;
        }
    }
}