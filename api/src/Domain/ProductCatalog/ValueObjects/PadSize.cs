using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Common;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSize : Entity<PadSize> {
        public Measurement Diameter { get; }
        public Measurement? Thickness { get; }

        public PadSize(Measurement diameter, Measurement? thickness) {
            Id = Guid.NewGuid();
            Diameter = diameter;
            Thickness = thickness;
        }

        public PadSize(Guid id, Measurement diameter, Measurement? thickness) {
            Id = id;
            Diameter = diameter;
            Thickness = thickness;
        }
    }
}