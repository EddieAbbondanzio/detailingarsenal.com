using System;

namespace DetailingArsenal {
    /// <summary>
    /// Attribute to assist in registering dependencies with the DI container.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DependencyInjectionAttribute : System.Attribute {
        public Type? RegisterAs { get; set; }
        public LifeTime LifeTime { get; set; } = LifeTime.Transient; // Sometimes it's better to be explicit
        public Environment Environment { get; set; } = Environment.Any;
    }

    public enum LifeTime {
        Transient,
        Scoped,
        Singleton
    }

    [Flags]
    public enum Environment {
        Any = 0,
        Dev = 1,
        Prod = 1 << 1
    }
}