using System.Collections.Generic;
using System.Linq;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadColorUpdateRaw : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public PadColorImageUpdate? Image { get; set; }
        public List<PadOptionCreateOrUpdate> Options { get; set; } = null!;
    }
}