using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesUpdateCommand(Guid Id, string Name, Guid BrandId, List<PadSeriesSize> Sizes, List<PadCreateOrUpdate> Pads) : IAction;
}