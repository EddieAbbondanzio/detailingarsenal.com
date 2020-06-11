using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class ServiceConfigurationDto : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid? VehicleCategoryId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }
}