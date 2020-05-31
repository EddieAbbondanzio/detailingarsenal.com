using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace DetailingArsenal.Api {
    public class Program {
        public static async Task Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting web host");
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                await bus.Dispatch(new StartupEvent());

                // Perform any database migrations
                // scope.ServiceProvider.GetRequiredService<DatabaseMigrationRunner>().MigrateUp();
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
