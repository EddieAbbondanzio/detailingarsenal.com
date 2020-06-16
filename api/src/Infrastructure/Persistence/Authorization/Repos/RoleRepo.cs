using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Infrastructure.Persistence.Models;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class RoleRepo : DatabaseInteractor, IRoleRepo {
        public RoleRepo(IDatabase database) : base(database) { }

        public async Task<Role?> Find(string name) {
            var r = await Connection.QueryFirstOrDefaultAsync<Role>(
                @"select * from roles where name = @Name;",
                new {
                    Name = name
                }
            );

            if (r == null) {
                return null;
            }

            var perms = await Connection.QueryAsync<RolePermission>(
                @"select * from role_permissions where role_id = @Id",
                r
            );

            r.PermissionIds = perms.Select(p => p.PermissionId).ToList();
            return r;
        }

        public async Task<List<Role>> FindAll() {
            var roles = await Connection.QueryAsync<Role>(
                @"select * from roles"
            );

            foreach (Role role in roles) {
                var perms = await Connection.QueryAsync<RolePermission>(
                    @"select * from role_permissions where role_id = @Id",
                    role
                );

                role.PermissionIds = perms.Select(p => p.PermissionId).ToList();
            }

            return roles.ToList();
        }

        public async Task<Role?> FindById(Guid id) {
            var r = await Connection.QueryFirstOrDefaultAsync<Role>(
                @"select * from roles where id = @Id;",
                new {
                    Id = id
                }
            );

            if (r == null) {
                return null;
            }

            var perms = await Connection.QueryAsync<RolePermission>(
                @"select * from role_permissions where role_id = @Id",
                r
            );

            r.PermissionIds = perms.Select(p => p.PermissionId).ToList();
            return r;
        }


        public async Task<List<Role>> FindForUser(User user) {
            var userRoles = (await Connection.QueryAsync<Guid>(
                "select role_id from user_roles where user_id = @Id;",
                user
            )).ToList();

            var roles = await Connection.QueryAsync<Role>(
                "select * from roles where id = any(@Ids);",
                new { Ids = userRoles }
            );

            foreach (Role role in roles) {
                var perms = await Connection.QueryAsync<RolePermission>(
                    @"select * from role_permissions where role_id = @Id",
                    role
                );

                role.PermissionIds = perms.Select(p => p.PermissionId).ToList();
            }

            return roles.ToList();
        }

        public async Task Add(Role entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"insert into roles (id, name) values (@Id, @Name);", entity
                );

                var rolePerms = entity.PermissionIds.Select(p => new RolePermission() { PermissionId = p, RoleId = entity.Id });

                await Connection.ExecuteAsync(
                    @"insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);", rolePerms
                );

                t.Commit();
            }
        }

        public async Task Update(Role entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"update roles set name = @Name where id = @Id;", entity
                );

                await Connection.ExecuteAsync(@"delete from role_permissions where role_id = @Id;", entity);

                var rolePerms = entity.PermissionIds.Select(p => new RolePermission() { PermissionId = p, RoleId = entity.Id });

                await Connection.ExecuteAsync(
                    @"insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);", rolePerms
                );

                t.Commit();
            }
        }

        public async Task Delete(Role entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"delete from role_permissions where role_id = @Id",
                    entity
                );

                await Connection.ExecuteAsync(
                    @"delete from roles where id = @Id",
                    entity
                );

                t.Commit();
            }
        }

        public async Task AddToUser(User user, Role role) {
            var userRole = new UserRole() {
                UserId = user.Id,
                RoleId = role.Id
            };

            await Connection.ExecuteAsync(
                "insert into user_roles (user_id, role_id) values (@UserId, @RoleId);",
                userRole
            );
        }

        public async Task RemoveFromUser(User user, Role role) {
            await Connection.ExecuteAsync(
                @"delete from user_roles where user_id = @UserId and role_id = @RoleId",
                new {
                    UserId = user.Id,
                    RoleId = role.Id
                }
            );
        }
    }
}