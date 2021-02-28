using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public BrandReadModel Brand { get; }
        public List<string> PolisherTypes { get; }
        public List<PadSizeReadModel> Sizes { get; }
        public List<PadReadModel> Pads { get; }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, PolisherType? polisherTypes = null, List<PadSizeReadModel>? sizes = null, List<PadReadModel>? pads = null) {
            Id = id;
            Name = name;
            Brand = brand;
            PolisherTypes = BuildPolisherTypes(polisherTypes ?? PolisherType.None);
            Sizes = sizes ?? new();
            Pads = pads ?? new();
        }

        List<string> BuildPolisherTypes(PolisherType types) {
            List<string> t = new();

            if (types.HasFlag(PolisherType.DualAction)) {
                t.Add("dual_action");
            }

            if (types.HasFlag(PolisherType.LongThrow)) {
                t.Add("long_throw");
            }

            if (types.HasFlag(PolisherType.ForcedRotation)) {
                t.Add("forced_rotation");
            }

            if (types.HasFlag(PolisherType.Rotary)) {
                t.Add("rotary");
            }

            if (types.HasFlag(PolisherType.Mini)) {
                t.Add("mini");
            }

            return t;
        }
    }
}