using System;

namespace DetailingArsenal.Domain.Settings {
    public class UpdateServiceConfiguration : IDataTransferObject {
        public Guid? VehicleCategoryId { get; }
        public decimal Price { get; }
        public int Duration { get; }

        public UpdateServiceConfiguration(Guid? vehicleCategoryId, decimal price, int duration) {
            VehicleCategoryId = vehicleCategoryId;
            Price = price;
            Duration = duration;
        }
    }
}