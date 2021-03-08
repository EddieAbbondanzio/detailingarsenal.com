using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSizeReadModel(MeasurementReadModel Diameter, MeasurementReadModel? Thickness) : IDataTransferObject;
}