
using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Users {
    public class Auth0UserGateway : IUserGateway {
        private IAuth0ApiClientBuilder tokenGenerator;
        private IUserRepo userRepo;

        public Auth0UserGateway(IAuth0ApiClientBuilder tokenGenerator, IUserRepo userRepo) {
            this.tokenGenerator = tokenGenerator;
            this.userRepo = userRepo;
        }


        public async Task<Domain.Users.User?> GetUserByAuth0Id(string auth0Id) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.GetAsync(auth0Id);

            if (auth0User == null) {
                throw new EntityNotFoundException();
            }

            return await userRepo.FindByAuth0Id(auth0Id);
        }

        public async Task<Domain.Users.User> GetOrCreateUserByAuth0Id(string auth0Id) {
            if (auth0Id == "") {
                throw new ArgumentException("Auth0 Id is missing.");
            }

            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.GetAsync(auth0Id);

            if (auth0User == null) {
                throw new EntityNotFoundException();
            }

            var user = await userRepo.FindByAuth0Id(auth0Id);

            if (user == null) {
                user = Domain.Users.User.Create(
                    auth0Id,
                    auth0User.Email
                );

                try {
                    await userRepo.Add(user);
                    // await eventBus.Dispatch(new NewUserEvent(user));
                } catch (Exception) {
                    // When multiple requests are fired off after the user registers it could lead to multiple users being created.
                    Console.WriteLine("Duplicate user attempted to be created. Just use webhooks already");
                }

            }

            return user;
        }

        public async Task<Domain.Users.User?> GetUserById(Guid id) {
            var user = await userRepo.FindById(id);

            if (user == null) {
                return null;
            }

            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.GetAsync(user.Auth0Id);

            user.Email = auth0User.Email;
            return user;
        }

        public async Task<Domain.Users.User?> GetUserByEmail(string email) {
            var user = await userRepo.FindByEmail(email);

            if (user == null) {
                return null;
            }

            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = (await managementApiClient.Users.GetUsersByEmailAsync(email)).FirstOrDefault();

            if (auth0User == null) {
                return null;
            }

            user.Email = auth0User.Email;
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

            await userRepo.Add(user);
            // await eventBus.Dispatch(new NewUserEvent(user));

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