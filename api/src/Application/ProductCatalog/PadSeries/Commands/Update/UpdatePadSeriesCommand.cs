using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.ProductCatalog {
    public class UpdatePadSeriesCommand : IAction {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
        public List<PadInfo> Pads { get; set; } = new List<PadInfo>();
    }
}