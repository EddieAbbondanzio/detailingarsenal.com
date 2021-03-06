using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record BrandReadModel(Guid Id, string Name) : IDataTransferObject;
}