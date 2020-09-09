using System;

namespace DetailingArsenal.Domain.Users.Security {
    public class AuthorizationException : Exception {
        public AuthorizationException() : base("Unauthorized") { }
        public AuthorizationException(string message) : base(message) {
        }
    }
}