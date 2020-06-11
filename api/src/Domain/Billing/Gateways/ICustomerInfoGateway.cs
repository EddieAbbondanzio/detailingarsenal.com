using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ICustomerInfoGateway : IService {
        Task<CustomerInfo> FindByExternalId(string externalId);
        Task<CustomerInfo> Create(string email);
    }
}