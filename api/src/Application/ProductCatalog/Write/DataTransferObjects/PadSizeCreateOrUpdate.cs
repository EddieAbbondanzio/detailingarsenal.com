using System;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public class PadSizeCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public Measurement Diameter { get; }
        public Measurement? Thickness { get; }

        public PadSizeCreateOrUpdate(Guid? id, Measurement diameter, Measurement? thickness = null) {
            Id = id;
            Diameter = diameter;
            Thickness = thickness;
        }
    }
}