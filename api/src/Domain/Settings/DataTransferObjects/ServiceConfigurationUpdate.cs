using System;

namespace DetailingArsenal.Domain.Settings {
    public class ServiceConfigurationUpdate : IDataTransferObject {
        public Guid? VehicleCategoryId { get; }
        public decimal Price { get; }
        public int Duration { get; }

        public ServiceConfigurationUpdate(Guid? vehicleCategoryId, decimal price, int duration) {
            VehicleCategoryId = vehicleCategoryId;
            Price = price;
            Duration = duration;
        }
    }
}