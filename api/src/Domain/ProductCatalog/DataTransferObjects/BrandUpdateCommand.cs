using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record BrandUpdateCommand(Guid Id, string Name) : IAction;
}