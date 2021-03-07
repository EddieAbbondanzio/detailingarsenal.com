using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    [DependencyInjection]
    public class GetUserByAuth0IdHandler : ActionHandler<GetUserByAuth0IdQuery, UserReadModel> {
        IUserReader userReader;

        public GetUserByAuth0IdHandler(IUserReader userReader) {
            this.userReader = userReader;
        }

        public async override Task<UserReadModel> Execute(GetUserByAuth0IdQuery input, User? user) {
            var readModel = await userReader.ReadById(user!.Id);
            return readModel;
        }
    }
}