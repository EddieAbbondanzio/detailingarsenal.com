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
        internal static IEnumerable<(Type, DependencyInjectionAttribute)> FindAllDependencies(IEnumerable<Assembly> assemblies) {
            List<(Type, DependencyInjectionAttribute)> dependencyTypes = new();

            foreach (var assembly in assemblies) {
                foreach (var type in assembly.GetTypes()) {
                    var attr = type.GetCustomAttribute<DependencyInjectionAttribute>();

                    if (attr != null) {
                        dependencyTypes.Add((type, attr));
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
        internal static void AddDependencies(IServiceCollection collection, IEnumerable<(Type, DependencyInjectionAttribute)> dependencies, IEnumerable<DependencyInjectionMiddleware> middlewares) {
            var env = GetEnvironment();

            foreach (var dependecy in dependencies) {
                // Skip over it if the environment isn't a match
                if (!env.HasFlag(dependecy.Item2.Environment)) {
                    continue;
                }

                // Run through each middleware
                foreach (var mw in middlewares) {
                    mw.BeforeEach(dependecy);
                }

                switch (dependecy.Item2.LifeTime) {
                    case LifeTime.Transient:
                        collection.AddTransient(dependecy.Item2.RegisterAs ?? dependecy.Item1, dependecy.Item1);
                        break;

                    case LifeTime.Scoped:
                        collection.AddScoped(dependecy.Item2.RegisterAs ?? dependecy.Item1, dependecy.Item1);
                        break;

                    case LifeTime.Singleton:
                        collection.AddSingleton(dependecy.Item2.RegisterAs ?? dependecy.Item1, dependecy.Item1);
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