
using System;
using System.Threading.Tasks;

public abstract class ActionMiddleware {
    public abstract Task Execute(IServiceProvider provider, ActionHandler handler, IAction input, User? user);
}