using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Users {
    public interface IUserReader : IReader {
        Task<UserReadModel> ReadById(Guid id);
    }
}