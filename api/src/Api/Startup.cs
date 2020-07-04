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
using AutoMapper;
using DetailingArsenal.Infrastructure;
using System.Text.Json.Serialization;
using Npgsql.Logging;
using Stripe;
using DetailingArsenal.Application.Settings;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Infrastructure.Users;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Application.Security;
using DetailingArsenal.Application.Clients;
using DetailingArsenal.Application.Calendar;
using DetailingArsenal.Application.Users;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Application.Common;
using DetailingArsenal.Domain.Common;
using DetailingArsenal.Persistence.Billing;
using DetailingArsenal.Persistence.Security;
using DetailingArsenal.Persistence.Settings;
using DetailingArsenal.Persistence.Users;
using DetailingArsenal.Persistence.Clients;
using DetailingArsenal.Persistence.Calendar;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Application.Billing;
using DetailingArsenal.Infrastructure.Billing;

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
            services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();
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

            // Common
            services.AddTransient<ActionHandler<StartupCommand>, StartupHandler>();
            services.AddTransient<SynchronizationSaga>();
            services.AddTransient<RunDatabaseMigrationsStep>();
            services.AddTransient<RefreshSubscriptionPlansStep>();
            services.AddTransient<CreateOrUpdateAdminStep>();

            services.AddTransient<NewUserSaga>();
            services.AddTransient<CreateUserStep>();
            services.AddTransient<CreateBusinessStep>();
            services.AddTransient<CreateHoursOfOperationStep>();
            services.AddTransient<CreateTrialCustomerStep>();

            services.AddTransient<IUserResolver, UserResolver>();

            if (environment.IsProduction()) {
                services.AddTransient<IDomainEventSubscriber<NewUserCreatedEvent>, EmailEdOnNewUser>();
            }

            // Email
            services.AddConfig<EmailConfig>(Configuration.GetSection("Email"));
            services.AddTransient<IEmailClient, SmtpEmailClient>();

            // Billing
            var stripeConfig = services.AddConfig<ISubscriptionConfig, StripeConfig>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = stripeConfig.SecretKey;
            services.AddTransient<ISubscriptionPlanRepo, SubscriptionPlanRepo>();
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ICustomerService, Domain.Billing.CustomerService>();
            services.AddTransient<ICustomerGateway, StripeCustomerGateway>();
            services.AddTransient<ISubscriptionPlanGateway, StripeSubscriptionPlanGateway>();
            services.AddTransient<ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>>, GetSubscriptionPlansHandler>();
            services.AddTransient<ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanDto>>, RefreshSubscriptionPlansHandler>();
            services.AddTransient<ISubscriptionPlanService, SubscriptionPlanService>();

            // Security
            services.AddTransient<IPermissionRepo, PermissionRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IRoleService, RoleService>();
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ActionHandler<UpdateUserCommand, UserDto>, UpdateUserHandler>();
            services.AddTransient<ActionHandler<GetUserByAuth0IdQuery, UserDto>, GetUserByAuth0IdHandler>();

            // Vehicle Categories
            services.AddTransient<VehicleCategoryNameUniqueSpecification>();
            services.AddTransient<VehicleCategoryNotInUseSpecification>();
            services.AddTransient<IVehicleCategoryRepo, VehicleCategoryRepo>();
            services.AddTransient<IVehicleCategoryService, VehicleCategoryService>();
            services.AddTransient<ActionHandler<GetVehicleCategoriesQuery, List<VehicleCategoryDto>>, GetVehicleCategoriesHandler>();
            services.AddTransient<CreateVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryDto>, CreateVehicleCategoryHandler>();
            services.AddTransient<UpdateVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryDto>, UpdateVehicleCategoryHandler>();
            services.AddTransient<DeleteVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<DeleteVehicleCategoryCommand>, DeleteVehicleCategoryHandler>();

            // Business
            services.AddTransient<IBusinessRepo, BusinessRepo>();
            services.AddTransient<IBusinessService, BusinessService>();
            services.AddTransient<ActionHandler<GetBusinessQuery, BusinessDto>, GetBusinessHandler>();
            services.AddTransient<UpdateBusinessValidator>();
            services.AddTransient<ActionHandler<UpdateBusinessCommand, BusinessDto>, UpdateBusinessHandler>();

            // Hours Of Operation
            services.AddTransient<IHoursOfOperationRepo, HoursOfOperationRepo>();
            services.AddTransient<IHoursOfOperationService, HoursOfOperationService>();
            services.AddTransient<ActionHandler<GetHoursOfOperationQuery, HoursOfOperationDto>, GetHoursOfOperationHandler>();
            services.AddTransient<UpdateHoursOfOperationValidator>();
            services.AddTransient<ActionHandler<UpdateHoursOfOperationCommand, HoursOfOperationDto>, UpdateHoursOfOperationHandler>();

            // Service
            services.AddTransient<ServiceNameUniqueSpecification>();
            services.AddTransient<ServiceNotInUseSpecification>();
            services.AddTransient<IServiceRepo, ServiceRepo>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<ActionHandler<GetServicesQuery, List<ServiceDto>>, GetServicesHandler>();
            services.AddTransient<ActionHandler<CreateServiceCommand, ServiceDto>, CreateServiceHandler>();
            services.AddTransient<CreateServiceValidator>();
            services.AddTransient<ActionHandler<UpdateServiceCommand, ServiceDto>, UpdateServiceHandler>();
            services.AddTransient<UpdateServiceValidator>();
            services.AddTransient<ActionHandler<DeleteServiceCommand>, DeleteServiceHandler>();

            // Clients
            services.AddTransient<ClientHasNoAppointmentsSpecification>();
            services.AddTransient<IClientRepo, ClientRepo>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ActionHandler<GetClientsQuery, List<ClientDto>>, GetClientsHandler>();
            services.AddTransient<ActionHandler<CreateClientCommand, ClientDto>, CreateClientHandler>();
            services.AddTransient<ActionHandler<UpdateClientCommand, ClientDto>, UpdateClientHandler>();
            services.AddTransient<ActionHandler<DeleteClientCommand, ClientDto>, DeleteClientHandler>();

            // Calendar
            services.AddTransient<IAppointmentRepo, AppointmentRepo>();
            services.AddTransient<IAppointmentService, AppointmentService>();
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
