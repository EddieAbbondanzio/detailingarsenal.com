using System;
using System.Collections.Generic;
using System.Linq;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadColorCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public string Name { get; }
        public PadCategory Category { get; }
        public DataUrlImage? Image { get; }
        public List<PadOptionCreateOrUpdate> Options { get; }

        public PadColorCreateOrUpdate(Guid? id, string name, PadCategory category, DataUrlImage? image, List<PadOptionCreateOrUpdate> options) {
            Id = id;
            Name = name;
            Category = category;
            Image = image;
            Options = options;
        }
    }
}