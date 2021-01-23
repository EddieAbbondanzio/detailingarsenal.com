using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExts {
    /*
    *
    * While these may seem redundant to services.Configure<TConfig>() it doesn't return
    * a wrapped config like the other one (IOptions<TConfig>)
    *
    */

    public static TImplementation AddConfig<TImplementation>(this IServiceCollection services, IConfiguration config) where TImplementation : class, new() {
        if (services == null) {
            throw new ArgumentNullException("config");
        }

        var c = new TImplementation();
        config.Bind(c);
        services.AddSingleton<TImplementation>(c);
        return c;
    }

    public static TImplementation AddConfig<TInterface, TImplementation>(this IServiceCollection services, IConfiguration config) where TImplementation : class, TInterface, new() where TInterface : class {
        if (services == null) {
            throw new ArgumentNullException("services");
        }

        if (config == null) {
            throw new ArgumentNullException("config");
        }

        var c = new TImplementation();
        config.Bind(c);
        services.AddSingleton<TInterface>(c);
        return c;
    }

    public static void AddJwtAuthentication(this IServiceCollection services) {
        throw new NotImplementedException();
    }
}