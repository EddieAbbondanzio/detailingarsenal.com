using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadDeleteCommand : IAction {
        public Guid Id { get; }

        public PadDeleteCommand(Guid id) {
            Id = id;
        }
    }
}