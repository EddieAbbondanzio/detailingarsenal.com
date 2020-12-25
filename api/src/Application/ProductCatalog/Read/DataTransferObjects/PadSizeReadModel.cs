using DetailingArsenal.Application.Common;
using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSizeReadModel(Guid Id, MeasurementReadModel Diameter, MeasurementReadModel? Thickness) : IDataTransferObject;
}