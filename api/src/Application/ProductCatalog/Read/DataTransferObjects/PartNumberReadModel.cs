using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PartNumberReadModel(Guid Id, string Value, string? Notes) : IDataTransferObject;
}