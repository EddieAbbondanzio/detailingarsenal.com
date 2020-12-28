using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Domain {
    public class ValidateBillingConfigStep : SagaStep {
        BillingConfigValidator configValidator;
        IBillingConfig config;

        public ValidateBillingConfigStep(BillingConfigValidator configValidator, IBillingConfig config) {
            this.configValidator = configValidator;
            this.config = config;
        }

        public async override Task Execute(SagaContext context) {
            var res = await configValidator.ValidateAsync(config);

            if (!res.IsValid) {
                throw new ValidationException(res);
            }
        }
    }
}