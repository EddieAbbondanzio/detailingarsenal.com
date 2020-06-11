using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure {
    public class Auth0ApiClientBuilder : IAuth0ApiClientBuilder {
        private Auth0Config config;

        private static Auth0ApiToken? token;

        public Auth0ApiClientBuilder(Auth0Config config) {
            this.config = config;
        }

        public Task<AuthenticationApiClient> GetAuthenticationApiClient() {
            return Task.FromResult(new AuthenticationApiClient(new Uri(config.Domain)));
        }

        public async Task<ManagementApiClient> GetManagementApiClient() {
            var authenticationApiClient = new AuthenticationApiClient(new Uri(config.Domain));

            if (token == null || token.IsExpired()) {
                // How to generate token: https://github.com/auth0/auth0.net/issues/171
                var response = await authenticationApiClient.GetTokenAsync(new ClientCredentialsTokenRequest() {
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    Audience = $"{config.Domain}api/v2/"
                });

                token = new Auth0ApiToken(response);
            }

            return new ManagementApiClient(token.AccessToken, new Uri($"{config.Domain}api/v2/"));
        }
    }
}