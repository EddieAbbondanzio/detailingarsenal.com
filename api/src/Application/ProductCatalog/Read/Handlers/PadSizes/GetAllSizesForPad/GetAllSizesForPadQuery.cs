using System;

namespace  DetailingArsenal.Application.ProductCatalog {
    public record GetAllSizesForPadQuery(Guid PadId) : IAction;
}