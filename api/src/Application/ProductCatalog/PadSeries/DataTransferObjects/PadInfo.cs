using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    // Gross.
    public class PadInfo {
        public string Name { get; set; } = null!;
        public PadCategory Category { get; set; }
        public string? Image { get; set; }
    }
}