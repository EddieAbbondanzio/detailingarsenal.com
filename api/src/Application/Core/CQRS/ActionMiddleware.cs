
using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public abstract class ActionMiddleware {
        public abstract Task Execute(IServiceProvider provider, ActionHandler handler, IAction input, User? user);
    }
}