using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSize : ValueObject<PadSize> {
        public float Diameter { get; }
        public float Thickness { get; }
        public string PartNumber { get; }

        public PadSize(float diameter, float thickness, string partNumber) {
            Diameter = diameter;
            Thickness = thickness;
            PartNumber = partNumber;
        }
    }
}