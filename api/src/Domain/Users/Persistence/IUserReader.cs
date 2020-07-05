using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users {
    public interface IUserReader : IReader {
        Task<UserReadModel> ReadById(Guid id);
    }
}