using System.Text.Json.Serialization;

namespace DetailingArsenal.Application {
    public class PagingOptions : IDataTransferObject {
        public int Offset => PageSize * PageNumber;
        public int PageNumber { get; }
        public int PageSize { get; }

        [JsonConstructor]
        public PagingOptions(int pageNumber, int pageSize) {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}