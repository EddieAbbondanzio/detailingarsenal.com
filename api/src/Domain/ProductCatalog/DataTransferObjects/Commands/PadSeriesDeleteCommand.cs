using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesDeleteCommand(Guid Id) : IAction;
}