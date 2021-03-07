
using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Infrastructure.Users {
    [DependencyInjection(RegisterAs = typeof(IUserGateway))]
    public class Auth0UserGateway : IUserGateway {
        private IAuth0ApiClientBuilder tokenGenerator;

        public Auth0Config Config { get; }

        public Auth0UserGateway(Auth0Config config, IAuth0ApiClientBuilder tokenGenerator) {
            Config = config;
            this.tokenGenerator = tokenGenerator;
        }


        public async Task<Domain.Users.User> GetUserByAuth0Id(string auth0Id) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();
            var auth0User = await managementApiClient.Users.GetAsync(auth0Id) ?? throw new EntityNotFoundException();

            var user = new Domain.Users.User(
                auth0User.UserId,
                auth0User.Email,
                auth0User.UserName,
                auth0User.CreatedAt ?? throw new NullReferenceException()
            );

            return user;
        }

        public async Task<Domain.Users.User> CreateUser(string email, string password) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.CreateAsync(new UserCreateRequest() {
                UserName = "Admin",
                Email = email,
                Password = password,
                Connection = Config.Connection
            });

            var user = new Domain.Users.User(
                auth0User.UserId,
                auth0User.Email,
                auth0User.UserName,
                auth0User.CreatedAt ?? throw new NullReferenceException()
            );

            return user;
        }

        public async Task UpdatePassword(Domain.Users.User user, string newPassword) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            await managementApiClient.Users.UpdateAsync(user.Auth0Id, new UserUpdateRequest() {
                Password = newPassword,
                Connection = Config.Connection
            });
        }
    }
}