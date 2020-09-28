using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadUpdateCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadUpdateCommand(Guid id, string name, Guid brandId, List<PadCreateOrUpdate> pads) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}