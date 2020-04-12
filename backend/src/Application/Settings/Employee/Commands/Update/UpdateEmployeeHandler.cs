using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validate(typeof(UpdateEmployeeValidator))]
    public class UpdateEmployeeHandler : ActionHandler<UpdateEmployeeCommand, EmployeeDto> {
        private IEmployeeRepo repo;
        private IMapper mapper;

        public UpdateEmployeeHandler(IEmployeeRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        protected async override Task<EmployeeDto> Execute(UpdateEmployeeCommand input, User? user) {
            var e = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            if (e.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            e.Name = input.Name;
            e.Position = input.Position;

            await repo.Update(e);
            return mapper.Map<Employee, EmployeeDto>(e);
        }
    }
}