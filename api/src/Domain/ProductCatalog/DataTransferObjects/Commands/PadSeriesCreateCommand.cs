using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesCreateCommand(string Name, Guid BrandId, List<PadSeriesSize> Sizes, List<PadCreateOrUpdate> Pads) : IAction;
}