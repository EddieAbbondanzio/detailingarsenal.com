using System;

namespace DetailingArsenal.Domain {
    public class AuthorizationException : Exception {
        public AuthorizationException(string message) : base(message) {
        }
    }
}