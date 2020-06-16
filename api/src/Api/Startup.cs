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
using DetailingArsenal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DetailingArsenal.Domain;
using AutoMapper;
using DetailingArsenal.Infrastructure;
using System.Text.Json.Serialization;
using Npgsql.Logging;
using Stripe;
using DetailingArsenal.Domain.Core;
using DetailingArsenal.Infrastructure.Core;

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
            services.AddCors();

            // Core
            services.AddSingleton<DomainEvents>();
            services.AddSingleton<IDomainEventSubscriberCollection, DomainEventSubscriberCollection>();

            // Auth0
            var authConfig = services.AddConfig<Auth0Config>(Configuration.GetSection("Auth0"));
            services.AddTransient<IAuth0ApiClientBuilder, Auth0ApiClientBuilder>();
            services.AddTransient<IUserGateway, Auth0UserGateway>();

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
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opts.JsonSerializerOptions.IgnoreNullValues = true;
            });

            // Infrastructure
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ActionMiddleware, AuthorizationMiddleware>();
            services.AddTransient<ActionMiddleware, ValidationMiddleware>();
            services.AddScoped<IEventBus, EventBus>();
            services.AddTransient<IBusEventHandler<StartupEvent>, RunMigrationsOnStartup>();


            // Mapping
            var mapperConfiguration = new MapperConfiguration(config => {
                config.CreateMap<Client, ClientDto>();
                config.CreateMap<User, UserDto>();
                config.CreateMap<Business, BusinessDto>();
                config.CreateMap<VehicleCategory, VehicleCategoryDto>();
                config.CreateMap<Service, ServiceDto>();
                config.CreateMap<ServiceConfiguration, ServiceConfigurationDto>();
                config.CreateMap<HoursOfOperation, HoursOfOperationDto>();
                config.CreateMap<HoursOfOperationDay, HoursOfOperationDayDto>();
                config.CreateMap<Appointment, AppointmentDto>();
                config.CreateMap<AppointmentBlock, AppointmentBlockDto>();
                config.CreateMap<Permission, PermissionDto>();
                config.CreateMap<Role, RoleDto>();
                config.CreateMap<SubscriptionPlanPrice, SubscriptionPlanPriceDto>();
                config.CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
            });
            services.AddSingleton<Domain.IMapper>(new AutoMapperAdapter(mapperConfiguration.CreateMapper()));

            // Email
            services.AddConfig<EmailConfig>(Configuration.GetSection("Email"));
            services.AddTransient<IEmailClient, SmtpEmailClient>();

            // Billing
            var stripeConfig = services.AddConfig<ISubscriptionConfig, StripeConfig>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = stripeConfig.SecretKey;
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ISubscriptionPlanRepo, SubscriptionPlanRepo>();
            services.AddTransient<ISubscriptionRepo, SubscriptionRepo>();
            services.AddTransient<ICustomerInfoGateway, StripeCustomerInfoGateway>();
            services.AddTransient<ISubscriptionGateway, StripeSubscriptionGateway>();
            services.AddTransient<ISubscriptionPlanInfoGateway, StripeSubscriptionPlanInfoGateway>();
            services.AddTransient<IBusEventHandler<NewUserEvent>, CreateCustomerAndStartTrialOnNewUser>();
            services.AddTransient<IBusEventHandler<StartupEvent>, RefreshSubscriptionPlansOnStartup>();
            services.AddTransient<ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>>, GetSubscriptionPlansHandler>();
            services.AddTransient<ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanDto>>, RefreshSubscriptionPlansHandler>();
            services.AddTransient<ActionHandler<UpdateSubscriptionPlanCommand, SubscriptionPlanDto>, UpdateSubscriptionPlanHandler>();
            services.AddTransient<Application.SubscriptionPlanService>();

            // Authorization
            services.AddTransient<IPermissionRepo, PermissionRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<PermissionUniqueSpecification>();
            services.AddTransient<PermissionNotInUseSpecification>();
            services.AddTransient<RoleNameUniqueSpecification>();
            services.AddTransient<CreatePermissionValidator>();
            services.AddTransient<UpdatePermissionValidator>();
            services.AddTransient<CreateRoleValidator>();
            services.AddTransient<UpdateRoleValidator>();
            services.AddTransient<ActionHandler<GetPermissionsQuery, List<PermissionDto>>, GetPermissionsHandler>();
            services.AddTransient<ActionHandler<GetUserPermissionsQuery, List<PermissionDto>>, GetUserPermissionsHandler>();
            services.AddTransient<ActionHandler<CreatePermissionCommand, PermissionDto>, CreatePermissionHandler>();
            services.AddTransient<ActionHandler<UpdatePermissionCommand, PermissionDto>, UpdatePermissionHandler>();
            services.AddTransient<ActionHandler<DeletePermissionCommand>, DeletePermissionHandler>();
            services.AddTransient<ActionHandler<GetRolesQuery, List<RoleDto>>, GetRolesHandler>();
            services.AddTransient<ActionHandler<CreateRoleCommand, RoleDto>, CreateRoleHandler>();
            services.AddTransient<ActionHandler<UpdateRoleCommand, RoleDto>, UpdateRoleHandler>();
            services.AddTransient<ActionHandler<DeleteRoleCommand>, DeleteRoleHandler>();
            services.AddTransient<ActionHandler<AddRoleToUserCommand>, AddRoleToUserHandler>();
            services.AddTransient<ActionHandler<RemoveRoleFromUserCommand>, RemoveRoleFromUserHandler>();

            // Database
            var dbConfig = services.AddConfig<IDatabaseConfig, PostgresDatabaseConfig>(Configuration.GetSection("Database"));
            services.AddScoped<IDatabaseMigrationRunner, FluentMigratorMigrationRunner>();
            services.AddSingleton<IDatabase, PostgresDatabase>();
            services.AddDatabaseMigrations(dbConfig.GetConnectionString(), typeof(MigrationsFlag).Assembly);

            // User
            services.AddConfig<AdminConfig>(Configuration.GetSection("Admin"));
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<ActionHandler<UpdateUserCommand, UserDto>, UpdateUserHandler>();
            services.AddTransient<ActionHandler<GetUserByAuth0IdQuery, UserDto>, GetUserByAuth0IdHandler>();
            services.AddTransient<IBusEventHandler<StartupEvent>, CreateOrUpdateAdminOnStartup>();

            if (environment.IsProduction()) {
                services.AddTransient<IBusEventHandler<NewUserEvent>, NotifyEdOfNewUser>();
            }

            // Vehicle Categories
            services.AddTransient<VehicleCategoryNameUniqueSpecification>();
            services.AddTransient<VehicleCategoryNotInuseSpecification>();
            services.AddTransient<IVehicleCategoryRepo, VehicleCategoryRepo>();
            services.AddTransient<ActionHandler<GetVehicleCategoriesQuery, List<VehicleCategoryDto>>, GetVehicleCategoriesHandler>();
            services.AddTransient<CreateVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryDto>, CreateVehicleCategoryHandler>();
            services.AddTransient<UpdateVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryDto>, UpdateVehicleCategoryHandler>();
            services.AddTransient<DeleteVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<DeleteVehicleCategoryCommand>, DeleteVehicleCategoryHandler>();

            // Business
            services.AddTransient<IBusinessRepo, BusinessRepo>();
            services.AddTransient<ActionHandler<GetBusinessQuery, BusinessDto>, GetBusinessHandler>();
            services.AddTransient<UpdateBusinessValidator>();
            services.AddTransient<ActionHandler<UpdateBusinessCommand, BusinessDto>, UpdateBusinessHandler>();
            services.AddTransient<IBusEventHandler<NewUserEvent>, NewUserBusinessCreator>();

            // Hours Of Operation
            services.AddTransient<IHoursOfOperationRepo, HoursOfOperationRepo>();
            services.AddTransient<ActionHandler<GetHoursOfOperationQuery, HoursOfOperationDto>, GetHoursOfOperationHandler>();
            services.AddTransient<ActionHandler<UpdateHoursOfOperationCommand, HoursOfOperationDto>, UpdateHoursOfOperationHandler>();
            services.AddTransient<IBusEventHandler<NewUserEvent>, NewUserHoursOfOperationCreator>();

            // Service
            services.AddTransient<ServiceNameUniqueSpecification>();
            services.AddTransient<ServiceNotInUseSpecification>();
            services.AddTransient<IServiceRepo, ServiceRepo>();
            services.AddTransient<ActionHandler<GetServicesQuery, List<ServiceDto>>, GetServicesHandler>();
            services.AddTransient<ActionHandler<CreateServiceCommand, ServiceDto>, CreateServiceHandler>();
            services.AddTransient<ActionHandler<UpdateServiceCommand, ServiceDto>, UpdateServiceHandler>();
            services.AddTransient<ActionHandler<DeleteServiceCommand>, DeleteServiceHandler>();

            // Clients
            services.AddTransient<ClientHasNoAppointmentsSpecification>();
            services.AddTransient<IClientRepo, ClientRepo>();
            services.AddTransient<ActionHandler<GetClientsQuery, List<ClientDto>>, GetClientsHandler>();
            services.AddTransient<ActionHandler<CreateClientCommand, ClientDto>, CreateClientHandler>();
            services.AddTransient<ActionHandler<UpdateClientCommand, ClientDto>, UpdateClientHandler>();
            services.AddTransient<ActionHandler<DeleteClientCommand, ClientDto>, DeleteClientHandler>();

            // Appointments
            services.AddTransient<IAppointmentRepo, AppointmentRepo>();
            services.AddTransient<ActionHandler<GetAppointmentsQuery, List<AppointmentDto>>, GetAppointmentsHandler>();
            services.AddTransient<ActionHandler<CreateAppointmentCommand, AppointmentDto>, CreateAppointmentHandler>();
            services.AddTransient<ActionHandler<UpdateAppointmentCommand, AppointmentDto>, UpdateAppointmentHandler>();
            services.AddTransient<ActionHandler<DeleteAppointmentCommand>, DeleteAppointmentHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
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
