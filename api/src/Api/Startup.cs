using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Text;
using DetailingArsenal.Application;
using DetailingArsenal.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DetailingArsenal.Domain;
using DetailingArsenal.Infrastructure;
using System.Text.Json.Serialization;
using Npgsql.Logging;
using Stripe;
using DetailingArsenal.Application.Settings;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Infrastructure.Users;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Application.Users.Security;
using DetailingArsenal.Application.Clients;
using DetailingArsenal.Application.Calendar;
using DetailingArsenal.Application.Users;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Persistence.Scheduling.Billing;
using DetailingArsenal.Persistence.Settings;
using DetailingArsenal.Persistence.Users;
using DetailingArsenal.Persistence.Clients;
using DetailingArsenal.Persistence.Calendar;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Application.Scheduling.Billing;
using DetailingArsenal.Infrastructure.Scheduling.Billing;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Persistence.ProductCatalog;
using DetailingArsenal.Persistence.Users.Security;
using DetailingArsenal.Application.Shared;
using DetailingArsenal.Persistence.Shared;
using DetailingArsenal.Domain.Shared;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Application.Admin.ProductCatalog;
using DetailingArsenal.Persistence.Admin.ProductCatalog;

namespace DetailingArsenal.Api {
    public class Startup {
        IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            environment = env;
            Configuration = configuration;
            SqlMapper.AddTypeHandler(new DateTimeHandler());
            SqlMapper.AddTypeHandler(new GuidHandler());
            // Add db logging
            // NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            Console.WriteLine($"Running in production: {environment.IsProduction()}");

            services.AddCors();

            DependencyInjectionRegistrar.Execute(
                services,
                AppDomain.CurrentDomain.GetAssemblies()
            );

            var authConfig = services.AddConfig<Auth0Config>(Configuration.GetSection("Auth0"));

            services.AddConfig<EmailConfig>(Configuration.GetSection("Email"));

            var stripeConfig = services.AddConfig<IBillingConfig, StripeConfig>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = stripeConfig.SecretKey;

            var dbConfig = services.AddConfig<DatabaseConfig, PostgresDatabaseConfig>(Configuration.GetSection("Database"));

            services.AddConfig<AdminConfig>(Configuration.GetSection("Admin"));

            services.AddAuthentication(opts => {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts => {
                opts.Authority = authConfig.Domain;
                opts.Audience = authConfig.Identifier;
            });

            services.AddControllers(cfg => {
                cfg.Filters.Add(typeof(ValidationExceptionMiddlware));
                cfg.Filters.Add(typeof(AuthorizationExceptionMiddlware));
                cfg.Filters.Add(typeof(SpecificationExceptionMiddleware));
            }).AddJsonOptions(opts => {
                opts.JsonSerializerOptions.Converters.Add(new EnumSnakeCaseConverter());
                opts.JsonSerializerOptions.Converters.Add(new EitherConverter());
                opts.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddDatabaseMigrations(dbConfig.GetConnectionString(), typeof(MigrationsFlag).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // Needed for NGINX
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseExceptionLogger();

            app.UseCors(b => {
                b.AllowAnyOrigin();
                b.AllowAnyMethod();
                b.AllowAnyHeader();
                b.WithExposedHeaders("Content-Range");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment()) {
                app.UseHttpsRedirection();
                // app.UseDeveloperExceptionPage(); // Can't use if we want to catch exceptions in custom middleware
            }

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
