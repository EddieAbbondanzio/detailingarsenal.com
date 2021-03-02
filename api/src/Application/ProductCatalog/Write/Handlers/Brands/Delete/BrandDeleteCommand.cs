using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record BrandDeleteCommand(Guid Id) : IAction;
}