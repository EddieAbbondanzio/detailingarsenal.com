using System;
using System.Reflection;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application {
    public class AuthorizationMiddleware : ActionMiddleware {
        private IRoleRepo roleRepo;
        private IPermissionRepo permissionRepo;

        public AuthorizationMiddleware(IRoleRepo roleRepo, IPermissionRepo permissionRepo) {
            this.roleRepo = roleRepo;
            this.permissionRepo = permissionRepo;
        }

#pragma warning disable 1998
        public async override Task Execute(IServiceProvider provider, ActionHandler handler, IAction input, User? user) {
            AuthorizationAttribute? attribute = handler.GetType().GetCustomAttribute<AuthorizationAttribute>();

            if (attribute != null) {
                if (attribute.Scope == null || attribute.Action == null) {
                    throw new ArgumentNullException("Authorization attribute improperly set up. Missing scope or action.");
                }

                if (user == null) {
                    throw new AuthorizationException("Authentication required.");
                }

                if (attribute.Action != null && attribute.Scope != null) {
                    var roles = await roleRepo.FindForUser(user);
                    var perms = await permissionRepo.FindForRoles(roles);

                    if (!perms.HasPermission(attribute.Action, attribute.Scope)) {
                        throw new AuthorizationException("Not permitted.");
                    }

                }
            }
        }
#pragma warning restore 1998
    }
}