namespace DetailingArsenal.Application.ProductCatalog {
    public class CreateBrandCommand : IAction {
        public string Name { get; set; } = null!;
    }
}