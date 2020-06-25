using System;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public class VehicleCategory : Aggregate<VehicleCategory>, IUserEntity {
        public const int NameMaxLength = 32;
        public const int DescriptionMaxLength = 128;

        public Guid UserId { get; set; }

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

        public static VehicleCategory Create(CreateVehicleCategory create, User user) {
            return new VehicleCategory() {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = create.Name,
                Description = create.Description
            };
        }
    }
}