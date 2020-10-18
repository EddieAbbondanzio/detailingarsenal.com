namespace DetailingArsenal.Application.ProductCatalog {
    public class PadSeriesSizeReadModel : IDataTransferObject {
        public float Diameter { get; }
        public float Thickness { get; }
        public string PartNumber { get; }

        public PadSeriesSizeReadModel(float diameter, float thickness, string partNumber) {
            Diameter = diameter;
            Thickness = thickness;
            PartNumber = partNumber;
        }
    }
}