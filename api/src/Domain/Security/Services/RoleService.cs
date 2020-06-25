using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Security {
    public interface IRoleService : IService {
        Task<Role> GetById(Guid id);
        Task<List<Role>> GetAll();
        Task<Role> Create(CreateRole create);
        Task Update(Role role, UpdateRole update);
        Task Delete(Role role);

        Task AddRoleToUser(Role role, User user);
        Task RemoveRoleFromUser(Role role, User user);
    }

    public class RoleService : IRoleService {
        IRoleRepo repo;
        RoleNameUniqueSpecification nameUniqueSpec;

        public RoleService(IRoleRepo repo, RoleNameUniqueSpecification nameUniqueSpec) {
            this.repo = repo;
            this.nameUniqueSpec = nameUniqueSpec;
        }

        public async Task<Role> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<List<Role>> GetAll() {
            return await repo.FindAll();
        }

        public async Task<Role> Create(CreateRole create) {
            var r = Role.Create(
                create.Name,
                create.PermissionIds
            );

            await nameUniqueSpec.CheckAndThrow(r);
            await repo.Add(r);

            return r;
        }

        public async Task Update(Role role, UpdateRole update) {
            role.Name = update.Name;
            role.PermissionIds = update.PermissionIds;

            await nameUniqueSpec.CheckAndThrow(role);
            await repo.Update(role);
        }

        public async Task Delete(Role role) {
            await repo.Delete(role);
        }

        public async Task AddRoleToUser(Role role, User user) {
            await repo.AddToUser(user, role);
        }

        public async Task RemoveRoleFromUser(Role role, User user) {
            await repo.RemoveFromUser(user, role);
        }
    }
}