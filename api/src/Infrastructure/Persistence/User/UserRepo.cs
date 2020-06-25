using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class UserRepo : DatabaseInteractor, IUserRepo {
        public UserRepo(IDatabase database) : base(database) { }

        public async Task<User?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<User>(
                @"select * from users where id = @Id", new { Id = id }
            );
        }

        public async Task<User?> FindByAuth0Id(string id) {
            return await Connection.QueryFirstOrDefaultAsync<User>(
                @"select * from users where auth_0_id = @Id", new { Id = id }
            );
        }

        public async Task<User?> FindByEmail(string email) {
            return await Connection.QueryFirstOrDefaultAsync<User>(
                @"select * from users where email = @Email", new { Email = email }
            );
        }

        public async Task Add(User entity) {
            await Connection.ExecuteAsync(
                @"insert into users (id, auth_0_id, name, email) VALUES (@Id, @Auth0Id, @Name, @Email);",
                entity
            );
        }

        public async Task Update(User entity) {
            await Connection.ExecuteAsync(
                @"update users set auth_0_id = @Auth0Id, name = @Name, email = @Email where id = @Id;",
                entity
            );
        }

        public async Task Delete(User entity) {
            await Connection.ExecuteAsync(
                "@delete from users where id = @Id;",
                entity
            );
        }
    }
}