using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Security {
    public class RoleRepo : DatabaseInteractor, IRoleRepo {
        public RoleRepo(IDatabase database) : base(database) { }

        public async Task<Role?> Find(string name) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync<Role>(
                    @"select * from roles where name = @Name;",
                    new {
                        Name = name
                    }
                );

                if (r == null) {
                    return null;
                }

                var perms = await conn.QueryAsync<RolePermission>(
                    @"select * from role_permissions where role_id = @Id",
                    r
                );

                r.PermissionIds = perms.Select(p => p.PermissionId).ToList();
                return r;
            }
        }

        public async Task<List<Role>> FindAll() {
            using (var conn = OpenConnection()) {
                var roles = await conn.QueryAsync<Role>(
                    @"select * from roles"
                );

                foreach (Role role in roles) {
                    var perms = await conn.QueryAsync<RolePermission>(
                        @"select * from role_permissions where role_id = @Id",
                        role
                    );

                    role.PermissionIds = perms.Select(p => p.PermissionId).ToList();
                }

                return roles.ToList();
            }
        }

        public async Task<Role?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync<Role>(
                    @"select * from roles where id = @Id;",
                    new {
                        Id = id
                    }
                );

                if (r == null) {
                    return null;
                }

                var perms = await conn.QueryAsync<RolePermission>(
                    @"select * from role_permissions where role_id = @Id",
                    r
                );

                r.PermissionIds = perms.Select(p => p.PermissionId).ToList();
                return r;
            }
        }


        public async Task<List<Role>> FindForUser(User user) {
            using (var conn = OpenConnection()) {
                var userRoles = (await conn.QueryAsync<Guid>(
                    "select role_id from user_roles where user_id = @Id;",
                    user
                )).ToList();

                var roles = await conn.QueryAsync<Role>(
                    "select * from roles where id = any(@Ids);",
                    new { Ids = userRoles }
                );

                foreach (Role role in roles) {
                    var perms = await conn.QueryAsync<RolePermission>(
                        @"select * from role_permissions where role_id = @Id",
                        role
                    );

                    role.PermissionIds = perms.Select(p => p.PermissionId).ToList();
                }

                return roles.ToList();
            }
        }

        public async Task Add(Role entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into roles (id, name) values (@Id, @Name);", entity
                    );

                    var rolePerms = entity.PermissionIds.Select(p => new RolePermission() { PermissionId = p, RoleId = entity.Id });

                    await conn.ExecuteAsync(
                        @"insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);", rolePerms
                    );

                    t.Commit();
                }
            }
        }

        public async Task Update(Role entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update roles set name = @Name where id = @Id;", entity
                    );

                    await conn.ExecuteAsync(@"delete from role_permissions where role_id = @Id;", entity);

                    var rolePerms = entity.PermissionIds.Select(p => new RolePermission() { PermissionId = p, RoleId = entity.Id });

                    await conn.ExecuteAsync(
                        @"insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);", rolePerms
                    );

                    t.Commit();
                }
            }
        }

        public async Task Delete(Role entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"delete from role_permissions where role_id = @Id",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"delete from roles where id = @Id",
                        entity
                    );

                    t.Commit();
                }
            }
        }

        public async Task AddToUser(User user, Role role, bool deleteExisting = false) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    if (deleteExisting) {
                        await conn.ExecuteAsync(
                            @"delete from user_roles where user_id = @Id;", user
                        );
                    }

                    var userRole = new UserRole() {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    await conn.ExecuteAsync(
                        "insert into user_roles (user_id, role_id) values (@UserId, @RoleId);",
                        userRole
                    );

                    t.Commit();
                }
            }
        }

        public async Task RemoveFromUser(User user, Role role) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"delete from user_roles where user_id = @UserId and role_id = @RoleId",
                    new {
                        UserId = user.Id,
                        RoleId = role.Id
                    }
                );
            }
        }
    }
}