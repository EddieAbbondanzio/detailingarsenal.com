using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class ReviewReadModel : IDataTransferObject {
        public Guid PadId { get; }
        public string Username { get; }
        public DateTime Date { get; }
        public int Stars { get; }
        public int? Cut { get; }
        public int? Finish { get; }
        public string Title { get; }
        public string Body { get; }

        public ReviewReadModel(Guid padId, string username, DateTime date, int stars, int? cut, int? finish, string title, string body) {
            PadId = padId;
            Username = username;
            Date = date;
            Stars = stars;
            Cut = cut;
            Finish = finish;
            Title = title;
            Body = body;
        }
    }
}