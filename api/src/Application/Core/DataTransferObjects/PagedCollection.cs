using System.Collections.Generic;

namespace DetailingArsenal.Application {
    public class PagedCollection<T> : IDataTransferObject {
        public Paging Paging { get; }
        public IEnumerable<T> Values { get; }

        public PagedCollection(Paging paging, IEnumerable<T> values) {
            Paging = paging;
            Values = values;
        }
    }
}