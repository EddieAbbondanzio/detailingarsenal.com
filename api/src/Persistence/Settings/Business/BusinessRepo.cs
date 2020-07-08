using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Settings {
    public class BusinessRepo : DatabaseInteractor, IBusinessRepo {
        public BusinessRepo(IDatabase database) : base(database) { }

        public async Task<Business?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<Business>(@"select * from businesses where id = @Id;", new { Id = id });
        }

        public async Task<Business?> FindByUser(User user) {
            return await Connection.QueryFirstOrDefaultAsync<Business>(@"select * from businesses where user_id = @Id;", user);
        }

        public async Task Add(Business entity) {
            await Connection.ExecuteAsync(
                @"insert into businesses (id, user_id, name, address, phone) VALUES (@Id, @UserId, @Name, @Address, @Phone);",
                entity
            );
        }

        public async Task Update(Business entity) {
            await Connection.ExecuteAsync(
                @"update businesses set name = @Name, address = @Address, phone = @Phone where id = @Id;",
                entity
            );
        }

        public async Task Delete(Business entity) {
            await Connection.ExecuteAsync(
                @"delete from businesses where id = @Id;",
                entity
            );
        }
    }
}