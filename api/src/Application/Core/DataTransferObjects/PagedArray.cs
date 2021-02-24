namespace DetailingArsenal.Application {
    public class PagedArray<T> : IDataTransferObject {
        public Paging Paging { get; }
        public T[] Values { get; }

        public PagedArray(Paging paging, T[] values) {
            Paging = paging;
            Values = values;
        }
    }
}