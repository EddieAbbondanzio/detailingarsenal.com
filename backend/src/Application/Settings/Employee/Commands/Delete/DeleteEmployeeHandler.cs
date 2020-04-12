using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validate(typeof(DeleteEmployeeValidator))]
    public class DeleteEmployeeHandler : ActionHandler<DeleteEmployeeCommand> {
        private IEmployeeRepo repo;

        public DeleteEmployeeHandler(IEmployeeRepo repo) {
            this.repo = repo;
        }

        protected async override Task Execute(DeleteEmployeeCommand command, User? user) {
            var employee = (await repo.FindById(command.Id)) ?? throw new EntityNotFoundException();

            if (employee.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await repo.Delete(employee);
        }
    }
}