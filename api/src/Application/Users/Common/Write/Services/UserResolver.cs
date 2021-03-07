using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public interface IUserResolver : IService {
        Task<User> Resolve(string auth0Id);
    }

    /// <summary>
    /// Application service to create, or resolve users.
    /// </summary>
    [DependencyInjection(RegisterAs = typeof(IUserResolver))]
    public class UserResolver : IUserResolver {
        IUserRepo userRepo;
        NewUserSaga newUserSaga;

        public UserResolver(IUserRepo userRepo, NewUserSaga newUserSaga) {
            this.userRepo = userRepo;
            this.newUserSaga = newUserSaga;
        }

        public async Task<User> Resolve(string auth0Id) {
            var user = await userRepo.FindByAuth0Id(auth0Id);

            // If no user found, sync with Auth0 and save it.
            if (user == null) {
                await newUserSaga.Execute(auth0Id);
                user = (await (userRepo.FindByAuth0Id(auth0Id)))!;
            }

            return user!;
        }
    }
}