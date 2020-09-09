using System;

namespace DetailingArsenal.Domain.Users {
    public class User : Aggregate<User> {
        public const int NameMaxLength = 64;
        public const int UsernameMinLength = 4;
        public const int UsernameMaxLength = 24;

        public string Auth0Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;

        public string? Name {
            get => name;
            set {
                if (value?.Length > NameMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                name = value;
            }
        }

        public DateTime JoinedDate { get; set; }

        private string? name;

        public static User Create(string auth0Id, string email) {
            return new User() {
                Id = Guid.NewGuid(),
                Auth0Id = auth0Id,
                Email = email
            };
        }
    }
}