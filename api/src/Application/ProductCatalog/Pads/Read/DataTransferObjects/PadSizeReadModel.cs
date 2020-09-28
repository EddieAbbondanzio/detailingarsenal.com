namespace DetailingArsenal.Application.ProductCatalog {
    public class PadSizeReadModel : IDataTransferObject {
        public float Diameter { get; }
        public float Thickness { get; }
        public string PartNumber { get; }

        public PadSizeReadModel(float diameter, float thickness, string partNumber) {
            Diameter = diameter;
            Thickness = thickness;
            PartNumber = partNumber;
        }
    }
}