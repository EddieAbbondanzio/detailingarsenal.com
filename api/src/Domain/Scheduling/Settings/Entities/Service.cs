using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class Service : Aggregate<Service>, IUserEntity {
        public const int NameMaxLength = 32;
        public const int DescriptionMaxLength = 512;

        public Guid UserId { get; set; }

        /// <summary>
        /// Display name of the service.
        /// </summary>
        public string Name {
            get => name;
            set {
                if (value.Length > NameMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                name = value;
            }
        }

        /// <summary>
        /// Longer description explaining the service.
        /// </summary>
        public string? Description {
            get => description;
            set {
                if (value?.Length > DescriptionMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                description = value;
            }
        }

        public ServicePricingMethod PricingMethod { get; set; }

        /// <summary>
        /// Possible configurations allowing for different durations and prices based on vehicle category.
        /// </summary>
        public List<ServiceConfiguration> Configurations { get; set; } = new List<ServiceConfiguration>();

        private string name = null!;
        private string? description = null;

        public static Service Create(Guid userId, string name, string? description, ServicePricingMethod pricingMethod) {
            return new Service() {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                Description = description,
                PricingMethod = pricingMethod
            };
        }
    }
}