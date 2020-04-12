using System;

namespace DetailingArsenal.Application {
    public class ServiceConfigurationDto : IDataTransferObject {
        // Id is omitted on purpose.
        public Guid? VehicleCategoryId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }
}