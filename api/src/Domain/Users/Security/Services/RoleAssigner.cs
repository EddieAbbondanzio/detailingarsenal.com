using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Users.Security {
    public interface IRoleAssigner : IService {
        Task ReplaceRoles(User user, string roleName);
        Task ReplaceRoles(User user, Guid roleId);
        Task AddRole(User user, string roleName);
        Task AddRole(User user, Guid roleId);
        Task RemoveRole(User user, string roleName);
        Task RemoveRole(User user, Guid roleId);
    }

    [DependencyInjection(RegisterAs = typeof(IRoleAssigner))]
    public class RoleAssigner : IRoleAssigner {
        IRoleRepo roleRepo;

        public RoleAssigner(IRoleRepo roleRepo) {
            this.roleRepo = roleRepo;
        }

        public async Task ReplaceRoles(User user, string roleName) {
            var role = await roleRepo.FindByName(roleName) ?? throw new EntityNotFoundException();
            await roleRepo.AddToUser(user, role, true);
        }

        public async Task ReplaceRoles(User user, Guid roleId) {
            var role = await roleRepo.FindById(roleId) ?? throw new EntityNotFoundException();
            await roleRepo.AddToUser(user, role, true);
        }

        public async Task AddRole(User user, string roleName) {
            var role = await roleRepo.FindByName(roleName) ?? throw new EntityNotFoundException();
            await roleRepo.AddToUser(user, role, false);
        }

        public async Task AddRole(User user, Guid roleId) {
            var role = await roleRepo.FindById(roleId) ?? throw new EntityNotFoundException();
            await roleRepo.AddToUser(user, role, false);
        }

        public async Task RemoveRole(User user, string roleName) {
            var role = await roleRepo.FindByName(roleName) ?? throw new EntityNotFoundException();
            await roleRepo.RemoveFromUser(user, role);
        }

        public async Task RemoveRole(User user, Guid roleId) {
            var role = await roleRepo.FindById(roleId) ?? throw new EntityNotFoundException();
            await roleRepo.RemoveFromUser(user, role);
        }
    }
}