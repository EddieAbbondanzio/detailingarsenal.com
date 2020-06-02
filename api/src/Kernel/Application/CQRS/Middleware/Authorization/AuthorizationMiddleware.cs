using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public class AuthorizationMiddleware : ActionMiddleware {
#pragma warning disable 1998
    public async override Task Execute(IServiceProvider provider, ActionHandler handler, IAction input, User? user) {
        AuthorizationAttribute? attribute = handler.GetType().GetCustomAttribute<AuthorizationAttribute>();

        if (attribute != null && user == null) {
            throw new AuthorizationException("REEEEEEE");
        }
    }
#pragma warning restore 1998
}