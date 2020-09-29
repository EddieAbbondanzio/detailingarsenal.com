using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesDeleteCommand : IAction {
        public Guid Id { get; }

        public PadSeriesDeleteCommand(Guid id) {
            Id = id;
        }
    }
}