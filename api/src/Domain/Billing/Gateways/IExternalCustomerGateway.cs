using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IExternalCustomerGateway : IService {
        Task<ExternalCustomer> FindByExternalId(string externalId);
        Task<ExternalCustomer> Create(string email);
    }
}