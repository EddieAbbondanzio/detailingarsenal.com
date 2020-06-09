using System.Threading.Tasks;

public interface ICustomerInfoService : IService {
    Task<CustomerInfo> FindByExternalId(string externalId);
    Task<CustomerInfo> Create(string email);
}