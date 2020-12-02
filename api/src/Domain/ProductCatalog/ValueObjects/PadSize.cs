using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSize : ValueObject<PadSize> {
        public float Diameter { get; }
        public float? Thickness { get; }

        public PadSize(float diameter, float? thickness) {
            Diameter = diameter;
            Thickness = thickness;
        }
    }
}