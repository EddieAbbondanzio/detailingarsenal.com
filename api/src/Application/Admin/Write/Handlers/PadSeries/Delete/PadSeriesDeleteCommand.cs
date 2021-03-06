using System;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record PadSeriesDeleteCommand(Guid Id) : IAction;
}