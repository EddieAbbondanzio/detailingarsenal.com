
using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Users {
    public class Auth0UserGateway : IUserGateway {
        private IAuth0ApiClientBuilder tokenGenerator;

        public Auth0UserGateway(IAuth0ApiClientBuilder tokenGenerator) {
            this.tokenGenerator = tokenGenerator;
        }


        public async Task<Domain.Users.User> GetUserByAuth0Id(string auth0Id) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();
            var auth0User = await managementApiClient.Users.GetAsync(auth0Id) ?? throw new EntityNotFoundException();

            var user = Domain.Users.User.Create(
                auth0Id,
                auth0User.Email
            );

            user.JoinedDate = auth0User.CreatedAt ?? throw new NullReferenceException();
            user.Username = auth0User.UserName;
            return user;
        }

        public async Task<Domain.Users.User> CreateUser(string email, string password) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.CreateAsync(new UserCreateRequest() {
                Email = email,
                Password = password,
                Connection = "email-pass-auth"
            });

            var user = Domain.Users.User.Create(
                auth0User.UserId,
                auth0User.Email
            );

            user.JoinedDate = auth0User.CreatedAt ?? throw new NullReferenceException();
            user.Username = auth0User.UserName;

            return user;
        }

        public async Task UpdatePassword(Domain.Users.User user, string newPassword) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            await managementApiClient.Users.UpdateAsync(user.Auth0Id, new UserUpdateRequest() {
                Password = newPassword,
                Connection = "email-pass-auth"
            });
        }
    }
}