using System;

namespace DetailingArsenal.Domain {
    public class ServiceConfiguration : Entity<Service> {
        public Guid ServiceId { get; set; }
        public Guid? VehicleCategoryId { get; set; }

        /// <summary>
        /// The price of the service in USD.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Duration in minutes.
        /// </summary>
        public int Duration {
            get => duration;
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException();
                }

                duration = value;
            }
        }

        private int duration;

        public static ServiceConfiguration Create(Guid serviceId, Guid? vehicleCategoryId, decimal price, int duration) {
            return new ServiceConfiguration() {
                Id = Guid.NewGuid(),
                ServiceId = serviceId,
                VehicleCategoryId = vehicleCategoryId,
                Price = price,
                Duration = duration
            };
        }
    }
}