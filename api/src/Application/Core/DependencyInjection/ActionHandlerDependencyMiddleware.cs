using System;

namespace DetailingArsenal.Application {
    public class ActionHandlerDependencyMiddleware : DependencyInjectionMiddleware {
        public override void BeforeEach((Type, DependencyInjectionAttribute) dependecy) {
            // If the type is an action handler, auto create it's register as type
            // Ex: public class FooHandler : ActionHandler<FooCommand> will register as ActionHandler<FooCommand>
            if (typeof(ActionHandler).IsAssignableFrom(dependecy.Item1)) {
                dependecy.Item2.RegisterAs = dependecy.Item1.BaseType;
            }
        }
    }
}