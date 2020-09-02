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
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<Business>(@"select * from businesses where id = @Id;", new { Id = id });
            }
        }

        public async Task<Business?> FindByUser(User user) {
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<Business>(@"select * from businesses where user_id = @Id;", user);
            }
        }

        public async Task Add(Business entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"insert into businesses (id, user_id, name, address, phone) VALUES (@Id, @UserId, @Name, @Address, @Phone);",
                    entity
                );
            }
        }

        public async Task Update(Business entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"update businesses set name = @Name, address = @Address, phone = @Phone where id = @Id;",
                    entity
                );
            }
        }

        public async Task Delete(Business entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"delete from businesses where id = @Id;",
                    entity
                );
            }
        }
    }
}