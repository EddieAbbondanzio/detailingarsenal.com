using System;

namespace DetailingArsenal.Domain {
    public class VehicleCategory : Entity<VehicleCategory>, IUserEntity {
        public const int NameMaxLength = 32;
        public const int DescriptionMaxLength = 128;

        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// Display name.
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
        /// Longer description explaining the vehicle category.
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

        private string name = null!;
        private string? description = null;
    }
}