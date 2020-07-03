using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users {
    public interface IUserService : IService {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByEmail(string email);
        Task<User?> TryGetUserByEmail(string email);
        Task<User?> TryGetUserByAuth0Id(string auth0Id);
        Task<User> CreateUser(string auth0Id);
        Task<User> CreateAdminUser(string email, string password);
        Task UpdatePassword(User user, string newPassword);
        Task DeleteUser(User user);
    }

    public class UserService : IUserService {
        IUserGateway userGateway;
        IUserRepo userRepo;
        IDomainEventPublisher eventPublisher;

        public UserService(IUserGateway userGateway, IUserRepo userRepo, IDomainEventPublisher eventPublisher) {
            this.userGateway = userGateway;
            this.userRepo = userRepo;
            this.eventPublisher = eventPublisher;
        }

        public async Task<User> GetUserById(Guid id) {
            return await userRepo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<User> GetUserByEmail(string email) {
            return await userRepo.FindByEmail(email) ?? throw new EntityNotFoundException();
        }

        public async Task<User?> TryGetUserByEmail(string email) {
            return await userRepo.FindByEmail(email);
        }


        public async Task<User?> TryGetUserByAuth0Id(string auth0Id) {
            return await userRepo.FindByAuth0Id(auth0Id);
        }

        public async Task<User> CreateUser(string auth0Id) {
            var cachedUser = await userRepo.FindByAuth0Id(auth0Id);

            if (cachedUser != null) {
                return cachedUser;
            }

            var newUser = await userGateway.GetUserByAuth0Id(auth0Id);

            // TODO: Switch to webhooks to avoid this.
            try {
                await userRepo.Add(newUser);
            } catch {
                Console.WriteLine("Duplicate user attempted to be created. Just use webhooks already");
            }

            // refactor this. Events should come from entity?
            _ = eventPublisher.Dispatch(new NewUserCreatedEvent(newUser));

            return newUser;
        }

        public async Task<User> CreateAdminUser(string email, string password) {
            var user = await userGateway.CreateUser(email, password);
            await userRepo.Add(user);

            // Don't trigger new user event, since this SHOULD be used for admin only.

            return user;
        }

        public async Task UpdatePassword(User user, string newPassword) {
            await userGateway.UpdatePassword(user, newPassword);
        }

        public async Task DeleteUser(User user) {
            await userRepo.Delete(user);
        }

    }
}