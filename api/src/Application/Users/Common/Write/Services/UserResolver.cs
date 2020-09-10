using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Common;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public interface IUserResolver : IService {
        Task<User> Resolve(string auth0Id);
    }

    public class UserResolver : IUserResolver {
        IUserRepo userRepo;
        NewUserSaga newUserSaga;

        public UserResolver(IUserRepo userRepo, NewUserSaga newUserSaga) {
            this.userRepo = userRepo;
            this.newUserSaga = newUserSaga;
        }

        public async Task<User> Resolve(string auth0Id) {
            var user = await userRepo.FindByAuth0Id(auth0Id);

            if (user == null) {
                await newUserSaga.Execute(auth0Id);
                user = (await (userRepo.FindByAuth0Id(auth0Id)))!;
            }

            return user!;
        }
    }
}