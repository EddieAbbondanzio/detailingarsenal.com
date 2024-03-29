using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Application.ProductCatalog {
    public record ReviewCreateCommand : IAction {
        public Guid PadId { get; }
        public int Stars { get; }
        public int? Cut { get; }
        public int? Finish { get; }
        public string Title { get; }
        public string Body { get; }

        [JsonConstructor]
        public ReviewCreateCommand(Guid padId, int stars, int? cut, int? finish, string title, string body) {
            PadId = padId;
            Stars = stars;
            Cut = cut;
            Finish = finish;
            Title = title;
            Body = body;
        }
    }
}