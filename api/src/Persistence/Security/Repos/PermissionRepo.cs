using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Persistence.Security {
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


        public async Task<List<Permission>> FindForRoles(IEnumerable<Role> roles) {
            var rolePerms = await Connection.QueryAsync<Guid>(
                @"select distinct permission_id from role_permissions where role_id = any(@Ids);",
                new { Ids = roles.Select(r => r.Id).ToList() }
            );

            return (await Connection.QueryAsync<Permission>(
                @"select * from permissions where id = any(@Ids);",
                new { Ids = rolePerms.ToList() }
            )).ToList();
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

        public async Task<bool> IsInUse(Permission permission) {
            int rolePermCount = await Connection.ExecuteScalarAsync<int>(@"select count(*) from role_permissions where permission_id = @Id", permission);
            return rolePermCount > 0;
        }
    }
}