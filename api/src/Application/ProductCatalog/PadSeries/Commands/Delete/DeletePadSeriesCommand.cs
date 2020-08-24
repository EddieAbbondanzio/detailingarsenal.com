using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class DeletePadSeriesCommand : IAction {
        public Guid Id { get; set; }
    }
}