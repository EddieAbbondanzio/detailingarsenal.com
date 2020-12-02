using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class ReviewRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PadId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Stars { get; set; }
        public int? Cut { get; set; }
        public int? Finish { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}