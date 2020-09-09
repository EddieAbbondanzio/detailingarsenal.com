using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Users.Security {
    public interface IPermissionService : IService {
        Task<List<Permission>> GetAll();
        Task<Permission> GetById(Guid id);
        Task<PermissionSet> GetForRoles(IEnumerable<Role> roles);

        Task<Permission> Create(PermissionCreate create, User user);
        Task Update(Permission permission, PermissionUpdate update);
        Task Delete(Permission permission);
    }

    public class PermissionService : IPermissionService {
        PermissionUniqueSpecification permissionUniqueSpec;
        PermissionNotInUseSpecification permissionNotInUseSpec;
        IPermissionRepo repo;

        public PermissionService(PermissionUniqueSpecification permissionUniqueSpec, PermissionNotInUseSpecification notInUseSpec, IPermissionRepo repo) {
            this.permissionUniqueSpec = permissionUniqueSpec;
            this.permissionNotInUseSpec = notInUseSpec;
            this.repo = repo;
        }

        public async Task<List<Permission>> GetAll() {
            return await repo.FindAll();
        }

        public async Task<Permission> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<PermissionSet> GetForRoles(IEnumerable<Role> roles) {
            return await repo.FindForRoles(roles);
        }

        public async Task<Permission> Create(PermissionCreate create, User user) {
            var p = Permission.Create(
                create.Action,
                create.Scope
            );

            await permissionUniqueSpec.CheckAndThrow(p);

            await repo.Add(p);
            return p;
        }

        public async Task Update(Permission permission, PermissionUpdate update) {
            permission.Action = update.Action;
            permission.Scope = update.Scope;

            await permissionUniqueSpec.CheckAndThrow(permission);
            await repo.Update(permission);
        }

        public async Task Delete(Permission permission) {
            await permissionNotInUseSpec.CheckAndThrow(permission);
            await repo.Delete(permission);
        }
    }
}