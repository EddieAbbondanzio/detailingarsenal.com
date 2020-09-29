using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateOrUpdate : IDataTransferObject {
        public string Name { get; set; }
        public PadCategory Category { get; set; }
        public PadMaterial Material { get; set; }
        public PadTexture Texture { get; set; }
        public List<PadSize> Sizes { get; set; }
        public List<PolisherType> PolisherTypes { get; set; }
        public DataUrlImage? Image { get; set; }

        public PadCreateOrUpdate() {
            // Used by API layer

            Name = null!;
            Sizes = new List<PadSize>();
            PolisherTypes = new List<PolisherType>();
        }

        public PadCreateOrUpdate(string name, PadCategory category, PadMaterial material, PadTexture texture, List<PadSize> sizes, List<PolisherType> polisherTypes, DataUrlImage? image) {
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Sizes = sizes;
            PolisherTypes = polisherTypes;
            Image = image;
        }
    }
}