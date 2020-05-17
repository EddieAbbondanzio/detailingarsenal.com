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
    }
}