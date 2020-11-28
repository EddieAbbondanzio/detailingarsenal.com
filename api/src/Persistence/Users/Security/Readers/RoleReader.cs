using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using DetailingArsenal.Application.Users.Security;

namespace DetailingArsenal.Persistence.Users.Security {
    public class RoleReader : DatabaseInteractor, IRoleReader {
        public RoleReader(IDatabase database) : base(database) { }

        public async Task<List<RoleReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(@"
                    select * roles;
                    select * from role_permissions;
                    select * from permissions;
                ")) {
                    return new();
                }
            }
        }

        public async Task<RoleReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(@"
                    select * from roles where id = @Id;
                    select p.* from roles r join role_permissions rp on r.id = rp.role_id join permissions p on rp.permission_id = p.id;
                ",
                new { Id = id })) {
                    var roleRow = reader.ReadFirstOrDefault<RoleRow>();

                    if (roleRow == null) {
                        return null;
                    }

                    var perms = reader.Read<PermissionRow>().Select(p => new PermissionReadModel(p.Id, p.Action, p.Scope)).ToList();
                    return new RoleReadModel(roleRow.Id, roleRow.Name, perms);
                }
            }
        }

        public async Task<RoleReadModel?> ReadByName(string name) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(@"
                    select * from roles where name = @Name;
                    select p.* from roles r join role_permissions rp on r.id = rp.role_id join permissions p on rp.permission_id = p.id;
                ",
                new { Name = name })) {
                    var roleRow = reader.ReadFirstOrDefault<RoleRow>();

                    if (roleRow == null) {
                        return null;
                    }

                    var perms = reader.Read<PermissionRow>().Select(p => new PermissionReadModel(p.Id, p.Action, p.Scope)).ToList();
                    return new RoleReadModel(roleRow.Id, roleRow.Name, perms);
                }
            }
        }
    }
}