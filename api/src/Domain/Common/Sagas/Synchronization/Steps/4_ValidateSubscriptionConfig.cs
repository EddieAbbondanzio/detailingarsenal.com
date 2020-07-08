using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Domain.Common {
    public class ValidateSubscriptionConfigStep : SagaStep {
        SubscriptionConfigValidator configValidator;
        ISubscriptionConfig config;

        public ValidateSubscriptionConfigStep(SubscriptionConfigValidator configValidator, ISubscriptionConfig config) {
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