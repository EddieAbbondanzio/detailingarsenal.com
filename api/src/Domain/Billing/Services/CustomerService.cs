using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public interface ICustomerService : IService {
        Task<Customer> CreateTrialCustomer(User user);
    }

}