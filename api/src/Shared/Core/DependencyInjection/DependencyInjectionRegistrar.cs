using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DetailingArsenal {
    public static class DependencyInjectionRegistrar {
        public static void Execute(IServiceCollection collection, IEnumerable<Assembly> assemblies) {
            var dependencyTypes = FindAllDependencies(assemblies);
            var middlewares = FindAllMiddlewares(assemblies);

            AddDependencies(collection, dependencyTypes, middlewares);
        }

        /// <summary>
        /// Find every type in a collection of assemblies that has the DI attribute
        /// defined on it's class.
        /// </summary>
        internal static IEnumerable<DependencyInjectionEntry> FindAllDependencies(IEnumerable<Assembly> assemblies) {
            List<DependencyInjectionEntry> dependencyTypes = new();

            foreach (var assembly in assemblies) {
                foreach (var type in assembly.GetTypes()) {
                    var attr = type.GetCustomAttribute<DependencyInjectionAttribute>();

                    if (attr != null) {
                        dependencyTypes.Add(new(type, attr));
                    }
                }
            }

            return dependencyTypes;
        }

        internal static IEnumerable<DependencyInjectionMiddleware> FindAllMiddlewares(IEnumerable<Assembly> assemblies) {
            List<DependencyInjectionMiddleware> mws = new();

            foreach (var assembly in assemblies) {
                foreach (var type in assembly.GetTypes()) {
                    if (typeof(DependencyInjectionMiddleware).IsAssignableFrom(type) && !type.IsAbstract) {
                        mws.Add(Activator.CreateInstance(type) as DependencyInjectionMiddleware ?? throw new NotSupportedException());
                    }
                }
            }

            return mws;
        }

        /// <summary>
        /// Add each dependency to the service collection with the appropriate lifetime.
        /// </summary>
        internal static void AddDependencies(IServiceCollection collection, IEnumerable<DependencyInjectionEntry> dependencies, IEnumerable<DependencyInjectionMiddleware> middlewares) {
            var env = GetEnvironment();

            foreach (var dependency in dependencies) {
                // Skip over it if the environment isn't a match
                if (!env.HasFlag(dependency.Attribute.Environment)) {
                    continue;
                }

                // Run through each middleware
                foreach (var mw in middlewares) {
                    mw.BeforeEach(dependency);
                }

                if (dependency.Attribute.RegisterAs != null && !dependency.Attribute.RegisterAs.IsAssignableFrom(dependency.Type)) {
                    throw new NotSupportedException($"Cannot register dependency. Cannot assign {dependency.Type.Name} from {dependency.Attribute.RegisterAs.Name}");
                }

                switch (dependency.Attribute.LifeTime) {
                    case LifeTime.Transient:
                        collection.AddTransient(dependency.Attribute.RegisterAs ?? dependency.Type, dependency.Type);
                        break;

                    case LifeTime.Scoped:
                        collection.AddScoped(dependency.Attribute.RegisterAs ?? dependency.Type, dependency.Type);
                        break;

                    case LifeTime.Singleton:
                        collection.AddSingleton(dependency.Attribute.RegisterAs ?? dependency.Type, dependency.Type);
                        break;
                }
            }
        }

        internal static Environment GetEnvironment() {
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            switch (env) {
                case "Development":
                    return Environment.Dev;
                case "Production":
                    return Environment.Prod;
                default:
                    throw new NotSupportedException($"Invalid environment: {env}");
            }
        }
    }
}