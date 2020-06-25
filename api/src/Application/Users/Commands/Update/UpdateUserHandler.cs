using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public class UpdateUserHandler : ActionHandler<UpdateUserCommand, UserDto> {
        private IUserRepo repo;
        private IMapper mapper;

        public UpdateUserHandler(IUserRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<UserDto> Execute(UpdateUserCommand input, User? user) {
            user!.Name = input.Name;
            await repo.Update(user);

            return mapper.Map<User, UserDto>(user);
        }
    }
}