using System;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record PartNumberReadModel(Guid Id, string Value, string? Notes) : IDataTransferObject;
}