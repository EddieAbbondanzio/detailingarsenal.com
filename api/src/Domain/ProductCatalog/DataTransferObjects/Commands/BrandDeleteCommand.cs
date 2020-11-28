using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record BrandDeleteCommand(Guid Id) : IAction;
}