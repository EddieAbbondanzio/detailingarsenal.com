using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class PermissionRepo : DatabaseInteractor, IPermissionRepo {
        public PermissionRepo(IDatabase database) : base(database) { }

        public async Task<Permission?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<Permission>(
                @"select * from permissions where id = @Id;",
                new { Id = id }
            );
        }

        public async Task<Permission?> Find(string action, string scope) {
            return await Connection.QueryFirstOrDefaultAsync<Permission>(
                @"select * from permissions where action = @Action and scope = @Scope;",
                new {
                    Action = action,
                    Scope = scope
                }
            );
        }

        public async Task<List<Permission>> FindAll() {
            return (await Connection.QueryAsync<Permission>(
                @"select * from permissions;"
            )).ToList();
        }

        public async Task Add(Permission entity) {
            await Connection.ExecuteAsync(
                @"insert into permissions (id, action, scope) values (@Id, @Action, @Scope);", entity
            );
        }

        public async Task Update(Permission entity) {
            await Connection.ExecuteAsync(
                @"update permissions set action = @Action, scope = @Scope where id = @Id", entity
            );
        }

        public async Task Delete(Permission entity) {
            await Connection.ExecuteAsync(
                @"delete from permissions where id = @Id", entity
            );
        }
    }
}