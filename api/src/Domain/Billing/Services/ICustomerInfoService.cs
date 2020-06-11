using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ICustomerInfoService : IService {
        Task<CustomerInfo> FindByExternalId(string externalId);
        Task<CustomerInfo> Create(string email);
    }
}