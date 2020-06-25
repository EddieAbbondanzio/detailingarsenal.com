using System;
using Auth0.AuthenticationApi.Models;

namespace DetailingArsenal.Infrastructure.Users {
    public class Auth0ApiToken {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime Expiration { get; }

        public Auth0ApiToken(AccessTokenResponse response) {
            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;
            Expiration = DateTime.Now.AddSeconds(response.ExpiresIn);
        }

        public bool IsExpired() {
            return DateTime.UtcNow > Expiration;
        }
    }
}