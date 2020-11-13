using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class ReviewReadModel : IDataTransferObject {
        public string Username { get; }
        public DateTime Date { get; }
        public int Stars { get; }
        public int? Cut { get; }
        public int? Finish { get; }
        public string Title { get; }
        public string Body { get; }

        public ReviewReadModel(string username, DateTime date, int stars, int? cut, int? finish, string title, string body) {
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