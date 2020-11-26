using DetailingArsenal;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class BrandCreateCommand : IAction {
        public string Name { get; set; }

        public BrandCreateCommand(string name) {
            Name = name;
        }
    }
}