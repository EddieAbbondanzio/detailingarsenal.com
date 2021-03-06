using System;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Review : Aggregate<Review>, IUserEntity {
        public const int TitleMaxLength = 64;
        public const int BodyMaxLength = 10_000;

        public Guid UserId { get; }
        public Guid PadId { get; }
        public DateTime CreatedDate { get; }
        public int Stars { get; set; }
        public int? Cut { get; set; }
        public int? Finish { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public Review(Guid userId, Guid padId, DateTime createdDate, int stars, int? cut, int? finish, string title, string body) {
            Id = Guid.NewGuid();
            UserId = userId;
            PadId = padId;
            CreatedDate = createdDate;
            Stars = stars;
            Cut = cut;
            Finish = finish;
            Title = title;
            Body = body;
        }
    }
}