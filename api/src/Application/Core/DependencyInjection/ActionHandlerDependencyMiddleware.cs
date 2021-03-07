using System;

namespace DetailingArsenal.Application {
    public class ActionHandlerDependencyMiddleware : DependencyInjectionMiddleware {
        public override void BeforeEach(DependencyInjectionEntry dependecy) {
            // If the type is an action handler, auto create it's register as type
            // Ex: public class FooHandler : ActionHandler<FooCommand> will register as ActionHandler<FooCommand>
            if (typeof(ActionHandler).IsAssignableFrom(dependecy.Type)) {
                dependecy.Attribute.RegisterAs = dependecy.Type.BaseType;
            }
        }
    }
}