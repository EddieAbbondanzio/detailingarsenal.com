using System;

namespace DetailingArsenal.Domain.Security {
    public class AuthorizationException : Exception {
        public AuthorizationException() : base("Unauthorized") { }
        public AuthorizationException(string message) : base(message) {
        }
    }
}