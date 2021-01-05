using System;

namespace DetailingArsenal.Api.ProductCatalog {
    public class ReviewCreateRequest : IDataTransferObject {
        public Guid PadId { get; set; }
        public int Stars { get; set; }
        public int? Cut { get; set; }
        public int? Finish { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}