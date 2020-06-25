using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class ClientRepo : DatabaseInteractor, IClientRepo {
        public ClientRepo(IDatabase database) : base(database) { }

        public async Task<Client?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<Client>(
                @"select * from clients where id = @Id", new { Id = id }
            );
        }

        public async Task<List<Client>> FindByUser(User user) {
            return (await Connection.QueryAsync<Client>(
                @"select * from clients where user_id = @Id;", user
            )).ToList();
        }

        public async Task Add(Client entity) {
            await Connection.ExecuteAsync(
                @"insert into clients (id, user_id, name, phone, email) values (@Id, @UserId, @Name, @Phone, @Email);", entity
            );
        }

        public async Task Update(Client entity) {
            await Connection.ExecuteAsync(
                @"update clients set name = @Name, phone = @Phone, email = @Email where id = @Id;", entity
            );
        }

        public async Task Delete(Client entity) {
            await Connection.ExecuteAsync(
                @"delete from clients where id = @Id", entity
            );
        }
    }
}