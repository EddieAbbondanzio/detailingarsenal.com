using System;

namespace DetailingArsenal.Domain.Settings {
    public class Business : Aggregate<Business>, IUserEntity {
        public const int NameMaxLength = 64;
        public const int AddressMaxLength = 128;
        public const int PhoneMaxLength = 32;

        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// Actual business name.
        /// </summary>
        public string? Name {
            get => name;
            set {
                if (value?.Length > NameMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                name = value;
            }
        }

        /// <summary>
        /// Physical address of the business.
        /// </summary>
        public string? Address {
            get => address;
            set {
                if (value?.Length > AddressMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                address = value;
            }
        }

        /// <summary>
        /// Contact phone number of the business.
        /// </summary>
        public string? Phone {
            get => phone;
            set {
                if (value?.Length > PhoneMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                phone = value;
            }
        }

        private string? name = null!;
        private string? address = null;
        private string? phone = null;

        public static Business Create(Guid userId) {
            return new Business() {
                Id = Guid.NewGuid(),
                UserId = userId
            };
        }
    }
}