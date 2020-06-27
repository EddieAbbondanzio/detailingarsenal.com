using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ICustomerGateway : IGateway {
        Task<ExternalCustomer> FindByExternalId(string externalId);
        Task<ExternalCustomer> Create(string email);
    }
}