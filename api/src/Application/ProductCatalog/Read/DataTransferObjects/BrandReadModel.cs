using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.ProductCatalog {
    public record BrandReadModel(Guid Id, string Name) : IDataTransferObject;
}