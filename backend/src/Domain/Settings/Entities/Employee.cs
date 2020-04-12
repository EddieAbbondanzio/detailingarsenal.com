using System;

namespace DetailingArsenal.Domain {
    public class Employee : Entity<Employee>, IUserEntity {
        public const int NameMaxLength = 64;
        public const int PositionMaxLength = 32;

        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// Legal, or nickname of the employee.
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
        /// The title of their job position.
        /// </summary>
        public string? Position {
            get => position;
            set {
                if (value?.Length > PositionMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                position = value;
            }
        }

        private string name = null!;
        private string? position = null;
    }
}