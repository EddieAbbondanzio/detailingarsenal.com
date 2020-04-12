using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public class Service : Entity<Service>, IUserEntity {
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

        /// <summary>
        /// Possible configurations allowing for different durations and prices based on vehicle category.
        /// </summary>
        public List<ServiceConfiguration> Configurations { get; set; } = new List<ServiceConfiguration>();

        private string name = null!;
        private string? description = null;
    }
}