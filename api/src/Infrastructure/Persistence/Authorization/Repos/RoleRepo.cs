using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

public class RoleRepo : DatabaseInteractor, IRoleRepo {
    public RoleRepo(IDatabase database) : base(database) { }

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

    public async Task Add(Role entity) {
        await Connection.ExecuteAsync(
            @"insert into roles (id, name) values (@Id, @Name);", entity
        );

        var rolePerms = entity.PermissionIds.Select(p => new RolePermission() { PermissionId = p, RoleId = entity.Id });

        await Connection.ExecuteAsync(
            @"insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);", rolePerms
        );
    }

    public async Task Update(Role entity) {
        await Connection.ExecuteAsync(
            @"update roles set name = @Name where id = @Id;", entity
        );

        await Connection.ExecuteAsync(@"delete from role_permissions where role_id = @Id;", entity);

        var rolePerms = entity.PermissionIds.Select(p => new RolePermission() { PermissionId = p, RoleId = entity.Id });

        await Connection.ExecuteAsync(
            @"insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);", rolePerms
        );
    }

    public async Task Delete(Role entity) {
        await Connection.ExecuteAsync(
            @"delete from role_permissions where role_id = @Id",
            entity
        );

        await Connection.ExecuteAsync(
            @"delete from roles where id = @Id",
            entity
        );
    }
}