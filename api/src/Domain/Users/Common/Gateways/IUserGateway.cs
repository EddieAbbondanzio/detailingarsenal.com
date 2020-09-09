using System;
using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users {
    public interface IUserGateway : IGateway {
        Task<User> GetUserByAuth0Id(string auth0Id);
        Task<User> CreateUser(string email, string password);
        Task UpdatePassword(User user, string newPassword);
    }
}