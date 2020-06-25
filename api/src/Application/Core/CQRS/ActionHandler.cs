using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application {
    public abstract class ActionHandler {
    }

    public abstract class ActionHandler<TInput> : ActionHandler where TInput : class, IAction {
        public abstract Task Execute(TInput input, User? user);
    }

    public abstract class ActionHandler<TInput, TOutput> : ActionHandler where TInput : class, IAction {
        public abstract Task<TOutput> Execute(TInput input, User? user);
    }
}