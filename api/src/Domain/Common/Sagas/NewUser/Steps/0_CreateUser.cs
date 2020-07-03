using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateUserStep : SagaStep<string> {
        IUserService userService;

        public CreateUserStep(IUserService userService) {
            this.userService = userService;
        }

        public async override Task Execute(SagaContext<string> context) {
            var user = await userService.CreateUser(context.Input);
            context.Data.User = user;

        }

        public async override Task Compensate(SagaContext<string> context) {
            await userService.DeleteUser(context.Data.User);
        }
    }
}