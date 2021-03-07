using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Clients {
    [DependencyInjection(RegisterAs = typeof(IClientRepo))]
    public class ClientRepo : DatabaseInteractor, IClientRepo {
        public ClientRepo(IDatabase database) : base(database) { }

        public async Task<Client?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<Client>(
                    @"select * from clients where id = @Id", new { Id = id }
                );
            }
        }

        public async Task<List<Client>> FindByUser(User user) {
            using (var conn = OpenConnection()) {
                return (await conn.QueryAsync<Client>(
                    @"select * from clients where user_id = @Id;", user
                )).ToList();
            }
        }

        public async Task Add(Client entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"insert into clients (id, user_id, name, phone, email) values (@Id, @UserId, @Name, @Phone, @Email);", entity
                );
            }
        }

        public async Task Update(Client entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"update clients set name = @Name, phone = @Phone, email = @Email where id = @Id;", entity
                );
            }
        }

        public async Task Delete(Client entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"delete from clients where id = @Id", entity
                );
            }
        }
    }
}