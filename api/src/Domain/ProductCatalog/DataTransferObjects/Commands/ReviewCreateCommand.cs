using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record ReviewCreateCommand(Guid PadId, int Stars, int? Cut, int? Finish, string Title, string Body) : IAction;
}