using System;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record PadSizeReadModel(Guid Id, MeasurementReadModel Diameter, MeasurementReadModel? Thickness) : IDataTransferObject;
}