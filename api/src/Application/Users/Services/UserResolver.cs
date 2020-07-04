using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Common;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public interface IUserResolver : IService {
        Task<User> Resolve(string auth0Id);
    }

    public class UserResolver : IUserResolver {
        IUserService userService;
        NewUserSaga newUserSaga;

        public UserResolver(IUserService userService, NewUserSaga newUserSaga) {
            this.userService = userService;
            this.newUserSaga = newUserSaga;
        }

        public async Task<User> Resolve(string auth0Id) {
            var user = await userService.TryGetUserByAuth0Id(auth0Id);

            if (user == null) {
                await newUserSaga.Execute(auth0Id);
                user = await (userService.TryGetUserByAuth0Id(auth0Id))!;
            }

            return user;
        }
    }
}