using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesSize : ValueObject<PadSeriesSize> {
        public float Diameter { get; }
        public float Thickness { get; }
        public string PartNumber { get; }

        public PadSeriesSize(float diameter, float thickness, string partNumber) {
            Diameter = diameter;
            Thickness = thickness;
            PartNumber = partNumber;
        }
    }
}