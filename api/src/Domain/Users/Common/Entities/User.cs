using System;

namespace DetailingArsenal.Domain.Users {
    public class User : Aggregate<User> {
        public const int NameMaxLength = 64;
        public const int UsernameMinLength = 4;
        public const int UsernameMaxLength = 24;

        public string Auth0Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime JoinedDate { get; set; }
        public string? Name { get; set; }

        /// <summary>
        /// Create a new user. Id, and joined date will be generated.
        /// </summary>
        /// <param name="auth0Id">External Auth0 id</param>
        /// <param name="email">Account / contact email</param>
        /// <param name="username">Unique username</param>
        public User(string auth0Id, string email, string username) {
            Id = Guid.NewGuid();
            Auth0Id = auth0Id;
            Email = email;
            Username = username;
            JoinedDate = new DateTime();
        }

        /// <summary>
        /// Rebuild an existing user.
        /// </summary>
        /// <param name="id">The user's unique id</param>
        /// <param name="auth0Id">External Auth0 id</param>
        /// <param name="email">Account / contact email</param>
        /// <param name="username">Unique username</param>
        /// <param name="joinedDate">Date the user first joined</param>
        /// <param name="name">Real name of the user</param>
        public User(Guid id, string auth0Id, string email, string username, DateTime joinedDate, string? name) {
            Id = id;
            Auth0Id = auth0Id;
            Email = email;
            Username = username;
            JoinedDate = joinedDate;
            Name = name;
        }
    }
}