using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadColor : Entity<PadColor> {
        public string Name { get; set; }
        public PadCategory Category { get; set; }
        public DataUrlImage? Image { get; set; }
        public List<PadOption> Options { get; set; }

        public PadColor(string name, PadCategory category, DataUrlImage? image = null, List<PadOption>? options = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Image = image;
            Options = options ?? new();
        }

        public PadColor(Guid id, string name, PadCategory category, DataUrlImage? image = null, List<PadOption>? options = null) {
            Id = id;
            Name = name;
            Category = category;
            Image = image;
            Options = options ?? new();
        }
    }
}