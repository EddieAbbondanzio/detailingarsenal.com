using System;

namespace DetailingArsenal.Domain.ProductCatalog {
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