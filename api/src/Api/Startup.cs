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
                config.CreateMap<Client, ClientView>();
                config.CreateMap<Business, BusinessView>();
                config.CreateMap<VehicleCategory, VehicleCategoryView>();
                config.CreateMap<Service, ServiceView>();
                config.CreateMap<ServiceConfiguration, ServiceConfigurationView>();
                config.CreateMap<HoursOfOperation, HoursOfOperationView>();
                config.CreateMap<HoursOfOperationDay, HoursOfOperationDayView>();
                config.CreateMap<Appointment, AppointmentView>();
                config.CreateMap<AppointmentBlock, AppointmentBlockView>();
                config.CreateMap<Permission, PermissionView>();
                config.CreateMap<Role, RoleView>();
                config.CreateMap<SubscriptionPlan, SubscriptionPlanView>();
                config.CreateMap<SubscriptionPlanPrice, SubscriptionPlanPriceView>()
                    .ForMember(v => v.BillingId, p => p.MapFrom(p => p.BillingReference.BillingId));
            });
            services.AddSingleton<Domain.IMapper>(new AutoMapperAdapter(mapperConfiguration.CreateMapper()));

            // Common
            services.AddTransient<ActionHandler<StartupCommand>, StartupHandler>();
            services.AddTransient<SynchronizationSaga>();
            services.AddTransient<RunDatabaseMigrationsStep>();
            services.AddTransient<RefreshSubscriptionPlansStep>();
            services.AddTransient<CreateOrUpdateAdminStep>();
            services.AddTransient<CreateRolesStep>();
            services.AddTransient<ValidateBillingConfigStep>();

            services.AddTransient<NewUserSaga>();
            services.AddTransient<CreateUserStep>();
            services.AddTransient<CreateBusinessStep>();
            services.AddTransient<CreateHoursOfOperationStep>();
            services.AddTransient<CreateSubscriptionStep>();
            services.AddTransient<AddRoleToNewUserStep>();

            services.AddTransient<IUserResolver, UserResolver>();

            if (environment.IsProduction()) {
                services.AddTransient<IDomainEventSubscriber<NewUserCreatedEvent>, EmailEdOnNewUser>();
            }

            // Email
            services.AddConfig<EmailConfig>(Configuration.GetSection("Email"));
            services.AddTransient<IEmailClient, SmtpEmailClient>();

            // Billing
            var stripeConfig = services.AddConfig<IBillingConfig, StripeConfig>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = stripeConfig.SecretKey;
            services.AddTransient<BillingConfigValidator>();
            services.AddTransient<ISubscriptionPlanRepo, SubscriptionPlanRepo>();
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ICustomerReader, CustomerReader>();
            services.AddTransient<ICustomerService, Domain.Billing.CustomerService>();
            services.AddTransient<ICustomerGateway, StripeCustomerGateway>();
            services.AddTransient<ISubscriptionPlanGateway, StripeSubscriptionPlanGateway>();
            services.AddTransient<ICheckoutSessionGateway, StripeCheckoutSessionGateway>();
            services.AddTransient<IBillingWebhookParser, StripeWebhookParser>();
            services.AddTransient<StripeWebhookConverter, CheckoutSessionCompletedConverter>();
            services.AddTransient<StripeWebhookConverter, CustomerSubscriptionTrialWillEndSoonConverter>();
            services.AddTransient<StripeWebhookConverter, CustomerSubscriptionInvoiceUpdatedConverter>();
            services.AddTransient<ActionHandler<GetCustomerQuery, CustomerReadModel>, GetCustomerHandler>();
            services.AddTransient<ActionHandler<CancelSubscriptionAtPeriodEndCommand>, CancelSubscriptionAtPeriodEndHandler>();
            services.AddTransient<ActionHandler<UndoCancellingSubscriptionCommand>, UndoCancellingSubscriptionHandler>();
            services.AddTransient<ActionHandler<GetDefaultSubscriptionPlanQuery, SubscriptionPlanView>, GetDefaultSubscriptionPlanHandler>();
            services.AddTransient<ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanView>>, GetSubscriptionPlansHandler>();
            services.AddTransient<ActionHandler<CreateCheckoutSessionCommand, BillingReference>, CreateSessionHandler>();
            services.AddTransient<ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanView>>, RefreshSubscriptionPlansHandler>();
            services.AddTransient<ISubscriptionPlanService, SubscriptionPlanService>();
            services.AddTransient<IDomainEventSubscriber<CheckoutSessionCompletedSuccessfully>, RefreshCustomerOnCheckoutSuccess>();
            services.AddTransient<IDomainEventSubscriber<CustomerTrialWillEndSoon>, EmailEdOnCustomerTrialWillEnd>();
            services.AddTransient<IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>, RefreshCustomerOnInvoiceUpdate>();
            services.AddTransient<IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>, EmailCustomerOnInvoicePaymentFailure>();
            services.AddTransient<IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>, UpdateRolesOnInvoiceUpdated>();

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
            services.AddTransient<ActionHandler<GetPermissionsQuery, List<PermissionView>>, GetPermissionsHandler>();
            services.AddTransient<ActionHandler<CreatePermissionCommand, PermissionView>, CreatePermissionHandler>();
            services.AddTransient<ActionHandler<UpdatePermissionCommand, PermissionView>, UpdatePermissionHandler>();
            services.AddTransient<ActionHandler<DeletePermissionCommand>, DeletePermissionHandler>();
            services.AddTransient<ActionHandler<GetRolesQuery, List<RoleView>>, GetRolesHandler>();
            services.AddTransient<ActionHandler<CreateRoleCommand, RoleView>, CreateRoleHandler>();
            services.AddTransient<ActionHandler<UpdateRoleCommand, RoleView>, UpdateRoleHandler>();
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
            services.AddTransient<IUserReader, UserReader>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ActionHandler<UpdateUserCommand>, UpdateUserHandler>();
            services.AddTransient<ActionHandler<GetUserByAuth0IdQuery, UserReadModel>, GetUserByAuth0IdHandler>();

            // Vehicle Categories
            services.AddTransient<VehicleCategoryNameUniqueSpecification>();
            services.AddTransient<VehicleCategoryNotInUseSpecification>();
            services.AddTransient<IVehicleCategoryRepo, VehicleCategoryRepo>();
            services.AddTransient<IVehicleCategoryService, VehicleCategoryService>();
            services.AddTransient<ActionHandler<GetVehicleCategoriesQuery, List<VehicleCategoryView>>, GetVehicleCategoriesHandler>();
            services.AddTransient<CreateVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryView>, CreateVehicleCategoryHandler>();
            services.AddTransient<UpdateVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryView>, UpdateVehicleCategoryHandler>();
            services.AddTransient<DeleteVehicleCategoryValidator>();
            services.AddTransient<ActionHandler<DeleteVehicleCategoryCommand>, DeleteVehicleCategoryHandler>();

            // Business
            services.AddTransient<IBusinessRepo, BusinessRepo>();
            services.AddTransient<IBusinessService, BusinessService>();
            services.AddTransient<ActionHandler<GetBusinessQuery, BusinessView>, GetBusinessHandler>();
            services.AddTransient<UpdateBusinessValidator>();
            services.AddTransient<ActionHandler<UpdateBusinessCommand, BusinessView>, UpdateBusinessHandler>();

            // Hours Of Operation
            services.AddTransient<IHoursOfOperationRepo, HoursOfOperationRepo>();
            services.AddTransient<IHoursOfOperationService, HoursOfOperationService>();
            services.AddTransient<ActionHandler<GetHoursOfOperationQuery, HoursOfOperationView>, GetHoursOfOperationHandler>();
            services.AddTransient<UpdateHoursOfOperationValidator>();
            services.AddTransient<ActionHandler<UpdateHoursOfOperationCommand, HoursOfOperationView>, UpdateHoursOfOperationHandler>();

            // Service
            services.AddTransient<ServiceNameUniqueSpecification>();
            services.AddTransient<ServiceNotInUseSpecification>();
            services.AddTransient<IServiceRepo, ServiceRepo>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<ActionHandler<GetServicesQuery, List<ServiceView>>, GetServicesHandler>();
            services.AddTransient<ActionHandler<CreateServiceCommand, ServiceView>, CreateServiceHandler>();
            services.AddTransient<CreateServiceValidator>();
            services.AddTransient<ActionHandler<UpdateServiceCommand, ServiceView>, UpdateServiceHandler>();
            services.AddTransient<UpdateServiceValidator>();
            services.AddTransient<ActionHandler<DeleteServiceCommand>, DeleteServiceHandler>();

            // Clients
            services.AddTransient<ClientHasNoAppointmentsSpecification>();
            services.AddTransient<IClientRepo, ClientRepo>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ActionHandler<GetClientsQuery, List<ClientView>>, GetClientsHandler>();
            services.AddTransient<ActionHandler<CreateClientCommand, ClientView>, CreateClientHandler>();
            services.AddTransient<ActionHandler<UpdateClientCommand, ClientView>, UpdateClientHandler>();
            services.AddTransient<ActionHandler<DeleteClientCommand, ClientView>, DeleteClientHandler>();

            // Calendar
            services.AddTransient<IAppointmentRepo, AppointmentRepo>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<ActionHandler<GetAppointmentsQuery, List<AppointmentView>>, GetAppointmentsHandler>();
            services.AddTransient<ActionHandler<CreateAppointmentCommand, AppointmentView>, CreateAppointmentHandler>();
            services.AddTransient<ActionHandler<UpdateAppointmentCommand, AppointmentView>, UpdateAppointmentHandler>();
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
