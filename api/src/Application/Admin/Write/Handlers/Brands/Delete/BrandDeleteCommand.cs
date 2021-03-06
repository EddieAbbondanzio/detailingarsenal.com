using System;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record BrandDeleteCommand(Guid Id) : IAction;
}