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
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<Permission>(
                    @"select * from permissions where id = @Id;",
                    new { Id = id }
                );
            }
        }

        public async Task<Permission?> Find(string action, string scope) {
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<Permission>(
                    @"select * from permissions where action = @Action and scope = @Scope;",
                    new {
                        Action = action,
                        Scope = scope
                    }
                );
            }
        }


        public async Task<PermissionSet> FindForRoles(IEnumerable<Role> roles) {
            using (var conn = OpenConnection()) {
                var rolePerms = await conn.QueryAsync<Guid>(
                    @"select distinct permission_id from role_permissions where role_id = any(@Ids);",
                    new { Ids = roles.Select(r => r.Id).ToList() }
                );

                var perms = await conn.QueryAsync<Permission>(
                    @"select * from permissions where id = any(@Ids);",
                    new { Ids = rolePerms.ToList() }
                );

                return new PermissionSet(perms);
            }
        }

        public async Task<List<Permission>> FindAll() {
            using (var conn = OpenConnection()) {
                return (await conn.QueryAsync<Permission>(
                    @"select * from permissions;"
                )).ToList();
            }
        }

        public async Task Add(Permission entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"insert into permissions (id, action, scope) values (@Id, @Action, @Scope);", entity
                );
            }
        }

        public async Task Update(Permission entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"update permissions set action = @Action, scope = @Scope where id = @Id", entity
                );
            }
        }

        public async Task Delete(Permission entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"delete from permissions where id = @Id", entity
                );
            }
        }

        public async Task<bool> IsInUse(Permission permission) {
            using (var conn = OpenConnection()) {
                int rolePermCount = await conn.ExecuteScalarAsync<int>(@"select count(*) from role_permissions where permission_id = @Id", permission);
                return rolePermCount > 0;
            }
        }
    }
}