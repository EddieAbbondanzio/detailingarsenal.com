using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class GetEmployeesHandler : ActionHandler<GetEmployeesQuery, List<EmployeeDto>> {
        private IEmployeeRepo repo;
        private IMapper mapper;

        public GetEmployeesHandler(IEmployeeRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        protected async override Task<List<EmployeeDto>> Execute(GetEmployeesQuery input, User? user) {
            var employees = await repo.FindByUser(user!);
            return mapper.Map<List<Employee>, List<EmployeeDto>>(employees);
        }
    }
}