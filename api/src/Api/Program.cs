using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DetailingArsenal.Api {
    public class Program {
        public static async Task Main(string[] args) {
            var host = CreateHostBuilder(args).ConfigureLogging((context, logging) => {
                logging.ClearProviders();

                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
            }).Build();

            using (var scope = host.Services.CreateScope()) {
                // Perform any database migrations
                scope.ServiceProvider.GetRequiredService<DatabaseMigrationRunner>().MigrateUp();
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
