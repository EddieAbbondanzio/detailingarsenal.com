using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadOptionReadModel(Guid PadSizeId, string? PartNumber = null) : IAction;
}