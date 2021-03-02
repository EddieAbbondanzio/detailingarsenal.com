using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesDeleteCommand(Guid Id) : IAction;
}