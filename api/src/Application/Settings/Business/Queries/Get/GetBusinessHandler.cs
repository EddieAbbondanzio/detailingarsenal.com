using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "businesses")]
    public class GetBusinessHandler : ActionHandler<GetBusinessQuery, BusinessDto> {
        private IBusinessRepo repo;
        private IMapper mapper;

        public GetBusinessHandler(IBusinessRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<BusinessDto> Execute(GetBusinessQuery query, User? user) {
            var b = await repo.FindByUser(user!);
            return mapper.Map<Business, BusinessDto>(b);
        }
    }
}