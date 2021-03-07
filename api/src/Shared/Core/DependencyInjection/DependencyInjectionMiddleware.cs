using System;

namespace DetailingArsenal {
    public abstract class DependencyInjectionMiddleware {
        public abstract void BeforeEach((Type, DependencyInjectionAttribute) dependecy);
    }
}