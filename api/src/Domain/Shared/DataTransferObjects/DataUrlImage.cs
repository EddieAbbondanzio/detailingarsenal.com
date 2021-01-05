using System;
using System.Text.RegularExpressions;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Domain.Shared {
    /// <summary>
    /// DataURL encoded image from the front end.
    /// </summary>
    public class DataUrlImage : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string Data { get; set; } = null!;

        public override bool Equals(object? obj) {
            return obj is DataUrlImage image &&
                   Name == image.Name &&
                   Data == image.Data;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Name, Data);
        }
    }
}