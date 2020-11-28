using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Users.Security {
    public interface IRoleAssigner : IService {

        Task AddRoleToUser(User user, string roleName, bool deleteExisting = false);
        Task AddRoleToUser(User user, Guid roleId, bool deleteExisting = false);
        Task RemoveRoleFromUser(User user, string roleName);
        Task RemoveRoleFromUser(User user, Guid roleId);
    }

    public class RoleAssigner : IRoleAssigner {
        IRoleRepo roleRepo;

        public RoleAssigner(IRoleRepo roleRepo) {
            this.roleRepo = roleRepo;
        }

        public async Task AddRoleToUser(User user, string roleName, bool deleteExisting = false) {
            var role = await roleRepo.FindByName(roleName) ?? throw new EntityNotFoundException();
            await roleRepo.AddToUser(user, role, deleteExisting);
        }

        public async Task AddRoleToUser(User user, Guid roleId, bool deleteExisting = false) {
            var role = await roleRepo.FindById(roleId) ?? throw new EntityNotFoundException();
            await roleRepo.AddToUser(user, role, deleteExisting);
        }

        public async Task RemoveRoleFromUser(User user, string roleName) {
            var role = await roleRepo.FindByName(roleName) ?? throw new EntityNotFoundException();
            await roleRepo.RemoveFromUser(user, role);
        }

        public async Task RemoveRoleFromUser(User user, Guid roleId) {
            var role = await roleRepo.FindById(roleId) ?? throw new EntityNotFoundException();
            await roleRepo.RemoveFromUser(user, role);
        }
    }
}