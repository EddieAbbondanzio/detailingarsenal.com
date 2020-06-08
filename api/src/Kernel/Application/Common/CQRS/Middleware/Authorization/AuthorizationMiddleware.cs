using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
            if (user == null) {
                throw new AuthorizationException("Authentication required.");
            }

            if (attribute.Action != null && attribute.Scope != null) {
                var roles = await roleRepo.FindForUser(user);
                var perms = await permissionRepo.FindForRoles(roles);

                foreach (Permission permission in perms) {
                    if (permission.Action == attribute.Action && permission.Scope == attribute.Scope) {
                        return;
                    }
                }

                throw new AuthorizationException("Not permitted.");
            }
        }
    }
#pragma warning restore 1998
}