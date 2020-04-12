using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validate(typeof(CreateEmployeeValidator))]
    public class CreateEmployeeHandler : ActionHandler<CreateEmployeeCommand, EmployeeDto> {
        private IEmployeeRepo repo;
        private IMapper mapper;

        public CreateEmployeeHandler(IEmployeeRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        protected async override Task<EmployeeDto> Execute(CreateEmployeeCommand input, User? user) {
            var employee = new Employee() {
                Id = Guid.NewGuid(),
                UserId = user!.Id,
                Name = input.Name,
                Position = input.Position
            };

            await repo.Add(employee);
            return mapper.Map<Employee, EmployeeDto>(employee);
        }
    }
}