using System;
using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IUserService : IService {
        Task<User?> GetUserByAuth0Id(string auth0Id);
        Task<User> GetOrCreateUserByAuth0Id(string auth0Id);
        Task<User?> GetUserById(string id);
        Task<User?> GetUserByEmail(string email);
        Task<User> CreateUser(string email, string password);
        Task UpdatePassword(User user, string newPassword);
    }
}