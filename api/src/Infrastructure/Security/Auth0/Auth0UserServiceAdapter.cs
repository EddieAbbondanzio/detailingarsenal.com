
using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure {
    public class Auth0UserServiceAdapter : IUserService {
        private IAuth0ApiClientBuilder tokenGenerator;
        private IUserRepo userRepo;
        private IEventBus eventBus;

        public Auth0UserServiceAdapter(IAuth0ApiClientBuilder tokenGenerator, IUserRepo userRepo, IEventBus eventBus) {
            this.tokenGenerator = tokenGenerator;
            this.userRepo = userRepo;
            this.eventBus = eventBus;
        }


        public async Task<Domain.User?> GetUserByAuth0Id(string auth0Id) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.GetAsync(auth0Id);

            if (auth0User == null) {
                throw new EntityNotFoundException();
            }

            return await userRepo.FindByAuth0Id(auth0Id);
        }

        public async Task<Domain.User> GetOrCreateUserByAuth0Id(string auth0Id) {
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
                user = new Domain.User() {
                    Id = Guid.NewGuid(),
                    Auth0Id = auth0Id,
                    Email = auth0User.Email
                };

                try {
                    await userRepo.Add(user);
                    await eventBus.Dispatch(new NewUserEvent(user));
                } catch (Exception) {
                    // When multiple requests are fired off after the user registers it could lead to multiple users being created.
                    Console.WriteLine("Duplicate user attempted to be created. Just use webhooks already");
                }

            }

            return user;
        }

        public async Task<Domain.User?> GetUserById(string id) {
            var user = await userRepo.FindById(new Guid(id));

            if (user == null) {
                return null;
            }

            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.GetAsync(user.Auth0Id);

            user.Email = auth0User.Email;
            return user;
        }

        public async Task<Domain.User?> GetUserByEmail(string email) {
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

        public async Task<Domain.User> CreateUser(string email, string password) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            var auth0User = await managementApiClient.Users.CreateAsync(new UserCreateRequest() {
                Email = email,
                Password = password,
                Connection = "email-pass-auth"
            });

            var user = new Domain.User() {
                Id = Guid.NewGuid(),
                Auth0Id = auth0User.UserId,
                Email = auth0User.Email
            };

            await userRepo.Add(user);
            await eventBus.Dispatch(new NewUserEvent(user));

            return user;
        }

        public async Task UpdatePassword(Domain.User user, string newPassword) {
            var managementApiClient = await tokenGenerator.GetManagementApiClient();

            await managementApiClient.Users.UpdateAsync(user.Auth0Id, new UserUpdateRequest() {
                Password = newPassword,
                Connection = "email-pass-auth"
            });
        }
    }
}