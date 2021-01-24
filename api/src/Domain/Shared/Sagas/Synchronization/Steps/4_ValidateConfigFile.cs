using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain {
    public class ValidateConfigFile : SagaStep {
        DatabaseConfigValidator databaseConfigValidator;
        DatabaseConfig databaseConfig;
        Auth0ConfigValidator auth0ConfigValidator;
        Auth0Config auth0Config;
        EmailConfigValidator emailConfigValidator;
        EmailConfig emailConfig;
        AdminConfigValidator adminConfigValidator;
        AdminConfig adminConfig;
        BillingConfigValidator billingConfigValidator;
        IBillingConfig billingConfig;

        public ValidateConfigFile(DatabaseConfigValidator databaseConfigValidator, DatabaseConfig databaseConfig, Auth0ConfigValidator auth0ConfigValidator, Auth0Config auth0Config, EmailConfigValidator emailConfigValidat, EmailConfig emailConfig, AdminConfigValidator adminConfigValidator, AdminConfig adminConfig, BillingConfigValidator configValidator, IBillingConfig config) {
            this.databaseConfigValidator = databaseConfigValidator;
            this.databaseConfig = databaseConfig;
            this.auth0ConfigValidator = auth0ConfigValidator;
            this.auth0Config = auth0Config;
            this.emailConfigValidator = emailConfigValidat;
            this.emailConfig = emailConfig;
            this.adminConfigValidator = adminConfigValidator;
            this.adminConfig = adminConfig;
            this.billingConfigValidator = configValidator;
            this.billingConfig = config;
        }

        public async override Task Execute(SagaContext context) {
            var dabaseConfigRes = await databaseConfigValidator.ValidateAsync(databaseConfig);
            if (!dabaseConfigRes.IsValid) {
                throw new ValidationException(dabaseConfigRes);
            }

            var auth0ConfigRes = await auth0ConfigValidator.ValidateAsync(auth0Config);
            if (!auth0ConfigRes.IsValid) {
                throw new ValidationException(auth0ConfigRes);
            }

            var emailConfigRes = await emailConfigValidator.ValidateAsync(emailConfig);
            if (!emailConfigRes.IsValid) {
                throw new ValidationException(emailConfigRes);
            }

            var adminConfigRes = await adminConfigValidator.ValidateAsync(adminConfig);
            if (!adminConfigRes.IsValid) {
                throw new ValidationException(adminConfigRes);
            }

            var billingConfigRes = await billingConfigValidator.ValidateAsync(billingConfig);
            if (!billingConfigRes.IsValid) {
                throw new ValidationException(billingConfigRes);
            }
        }
    }
}