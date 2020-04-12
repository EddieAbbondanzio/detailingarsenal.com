using System;

namespace DetailingArsenal.Domain {
    public sealed class Client : Entity<Client>, IUserEntity {
        public const int NameMaxLength = 64;
        public const int PhoneMaxLength = 32;
        public const int EmailMaxLength = 256 + 64;

        public Guid UserId { get; set; }

        /// <summary>
        /// Legal, or nickname of the client.
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
        /// Phone number of the client.
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

        /// <summary>
        /// Contact email of the client.
        /// </summary>
        public string? Email {
            get => email;
            set {
                if (value?.Length > EmailMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                email = value;
            }
        }

        private string name = null!;
        private string? phone;
        private string? email;
    }
}