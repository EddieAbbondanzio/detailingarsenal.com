using System.Collections.Generic;
using System.Linq;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    /// <summary>
    /// Enum free raw variant so we can deserialize our dirty snake 
    /// case enums ourselves.
    /// </summary>
    public class PadColorUpdate : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DataUrlImage? Image { get; set; }
        public List<PadOptionUpdate> Options { get; set; } = new();
    }
}