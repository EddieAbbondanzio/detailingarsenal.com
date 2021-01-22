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
                opts.JsonSerializerOptions.Converters.Add(new EnumSnakeCaseConverter());
                opts.JsonSerializerOptions.Converters.Add(new EitherConverter());
                opts.JsonSerializerOptions.IgnoreNullValues = true;
            });

            // Infrastructure
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ActionMiddleware, AuthorizationMiddleware>();
            services.AddTransient<ActionMiddleware, ValidationMiddleware>();

            // Common
            services.AddTransient<ActionHandler<StartupCommand>, StartupHandler>();
            services.AddTransient<SynchronizationSaga>();
            services.AddTransient<RunDatabaseMigrationsStep>();
            services.AddTransient<RefreshSubscriptionPlansStep>();
            services.AddTransient<CreateOrUpdateAdminStep>();
            services.AddTransient<CreateRolesStep>();
            services.AddTransient<ValidateBillingConfigStep>();
            services.AddTransient<GiveAdminAllPermissionsStep>();

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

            // Product Catalog
            services.AddTransient<ActionHandler<GetAllBrandsQuery, List<BrandReadModel>>, GetAllBrandsHandler>();
            services.AddTransient<ActionHandler<GetBrandByIdQuery, BrandReadModel?>, GetBrandByIdHandler>();
            services.AddTransient<ActionHandler<BrandCreateCommand, Guid>, BrandCreateHandler>();
            services.AddTransient<ActionHandler<BrandUpdateCommand>, UpdateBrandHandler>();
            services.AddTransient<ActionHandler<BrandDeleteCommand>, BrandDeleteHandler>();
            services.AddTransient<BrandCreateValidator>();
            services.AddTransient<BrandUpdateValidator>();
            services.AddTransient<IBrandReader, BrandReader>();
            services.AddTransient<BrandNameUniqueSpecification>();
            services.AddTransient<BrandNotInUseSpecification>();
            services.AddTransient<IBrandRepo, BrandRepo>();
            services.AddTransient<IPadSeriesRepo, PadSeriesRepo>();
            services.AddTransient<IPadSeriesReader, PadSeriesReader>();
            services.AddTransient<ActionHandler<GetPadSeriesByIdQuery, PadSeriesReadModel?>, GetPadSeriesByIdHandler>();
            services.AddTransient<ActionHandler<GetAllPadSeriesQuery, List<PadSeriesReadModel>>, GetAllPadSeriesHandler>();
            services.AddTransient<ActionHandler<PadSeriesCreateCommand, Guid>, PadSeriesCreateHandler>();
            services.AddTransient<ActionHandler<PadSeriesUpdateCommand>, PadSeriesUpdateHandler>();
            services.AddTransient<ActionHandler<PadSeriesDeleteCommand>, PadSeriesDeleteHandler>();
            services.AddTransient<ActionHandler<GetAllReviewsForPadQuery, List<ReviewReadModel>>, GetAllReviewsForPadHandler>();
            services.AddTransient<ActionHandler<GetReviewByIdQuery, ReviewReadModel?>, GetReviewByIdHandler>();
            services.AddTransient<ReviewCreateValidator>();
            services.AddTransient<PadSeriesPadSizeDiametersAreUniqueSpecification>();
            services.AddTransient<PadSeriesHasColorsSpecification>();
            services.AddTransient<PadSeriesHasOptionsForEveryColorSpecification>();
            services.AddTransient<PadSeriesHasSizesSpecification>();
            services.AddTransient<PadSeriesOptionsAreUniqueBySizesSpecification>();
            services.AddTransient<PadSeriesCreateOrUpdateCompositeSpecification>();
            services.AddTransient<ActionHandler<ReviewCreateCommand, Guid>, ReviewCreateHandler>();
            services.AddTransient<IReviewRepo, ReviewRepo>();
            services.AddTransient<IReviewReader, ReviewReader>();

            // Billing
            var stripeConfig = services.AddConfig<IBillingConfig, StripeConfig>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = stripeConfig.SecretKey;
            services.AddTransient<BillingConfigValidator>();
            services.AddTransient<ISubscriptionPlanRepo, SubscriptionPlanRepo>();
            services.AddTransient<ISubscriptionPlanReader, SubscriptionPlanReader>();
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ICustomerReader, CustomerReader>();
            services.AddTransient<ICustomerRefresher, Domain.Scheduling.Billing.CustomerRefresher>();
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
            services.AddTransient<ActionHandler<GetDefaultSubscriptionPlanQuery, SubscriptionPlanReadModel>, GetDefaultSubscriptionPlanHandler>();
            services.AddTransient<ActionHandler<GetByIdSubscriptionPlanQuery, SubscriptionPlanReadModel?>, GetByIdSubscriptionPlanHandler>();
            services.AddTransient<ActionHandler<GetAllSubscriptionPlansQuery, List<SubscriptionPlanReadModel>>, GetAllSubscriptionPlansHandler>();
            services.AddTransient<ActionHandler<CreateCheckoutSessionCommand, BillingReference>, CreateSessionHandler>();
            services.AddTransient<ActionHandler<RefreshSubscriptionPlansCommand>, RefreshSubscriptionPlansHandler>();
            services.AddTransient<ActionHandler<SubscriptionPlanUpdateCommand>, SubscriptionPlanUpdateHandler>();
            services.AddTransient<ISubscriptionPlanRefresher, SubscriptionPlanRefresher>();
            services.AddTransient<IDomainEventSubscriber<CheckoutSessionCompletedSuccessfully>, RefreshCustomerOnCheckoutSuccess>();
            services.AddTransient<IDomainEventSubscriber<CustomerTrialWillEndSoon>, EmailEdOnCustomerTrialWillEnd>();
            services.AddTransient<IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>, RefreshCustomerOnInvoiceUpdate>();
            services.AddTransient<IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>, EmailCustomerOnInvoicePaymentFailure>();
            services.AddTransient<IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>, UpdateRolesOnInvoiceUpdated>();

            // Security
            services.AddTransient<IPermissionRepo, PermissionRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<IPermissionReader, PermissionReader>();
            services.AddTransient<IRoleReader, RoleReader>();
            services.AddTransient<IRoleAssigner, RoleAssigner>();
            services.AddTransient<PermissionUniqueSpecification>();
            services.AddTransient<PermissionNotInUseSpecification>();
            services.AddTransient<RoleNameUniqueSpecification>();
            services.AddTransient<RolePermissionsDistinctSpecification>();
            services.AddTransient<RoleNotInUseSpecification>();
            services.AddTransient<CreatePermissionValidator>();
            services.AddTransient<UpdatePermissionValidator>();
            services.AddTransient<CreateRoleValidator>();
            services.AddTransient<UpdateRoleValidator>();
            services.AddTransient<ActionHandler<GetAllPermissionsQuery, List<PermissionReadModel>>, GetAllPermissionsHandler>();
            services.AddTransient<ActionHandler<GetPermissionByIdQuery, PermissionReadModel?>, GetPermissionByIdHandler>();
            services.AddTransient<ActionHandler<PermissionCreateCommand, Guid>, PermissionCreateHandler>();
            services.AddTransient<ActionHandler<PermissionUpdateCommand>, PermissionUpdateHandler>();
            services.AddTransient<ActionHandler<PermissionDeleteCommand>, PermissionDeleteHandler>();
            services.AddTransient<ActionHandler<GetAllRolesQuery, List<RoleReadModel>>, GetRolesHandler>();
            services.AddTransient<ActionHandler<GetRoleByIdQuery, RoleReadModel?>, GetRoleByIdHandler>();
            services.AddTransient<ActionHandler<RoleCreateCommand, Guid>, CreateRoleHandler>();
            services.AddTransient<ActionHandler<RoleUpdateCommand>, UpdateRoleHandler>();
            services.AddTransient<ActionHandler<RoleDeleteCommand>, DeleteRoleHandler>();
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
            services.AddTransient<ActionHandler<UserUpdateCommand>, UserUpdateHandler>();
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

            // Shared
            services.AddTransient<IImageReader, ImageReader>();
            services.AddSingleton<IImageProcessor, ImageProcessor>();
            services.AddTransient<ActionHandler<GetImageByIdQuery, SerializedImage?>, GetImageByIdHandler>();
            services.AddTransient<ActionHandler<GetThumbnailByIdQuery, SerializedImage?>, GetThumbnailByIdHandler>();
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
