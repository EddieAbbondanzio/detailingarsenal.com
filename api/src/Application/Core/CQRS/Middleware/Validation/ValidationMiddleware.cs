using System;
using System.Reflection;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DetailingArsenal.Application {
    [DependencyInjection(RegisterAs = typeof(ActionMiddleware))]
    public class ValidationMiddleware : ActionMiddleware {
        public async override Task Execute(IServiceProvider provider, ActionHandler handler, IAction input, User? user) {
            ValidationAttribute? attribute = handler.GetType().GetCustomAttribute<ValidationAttribute>();

            if (attribute != null) {
                var validator = (provider.GetRequiredService(attribute.Validator) as IValidator)!;

                var result = await validator.ValidateAsync(input);

                if (!result.IsValid) {
                    throw new ValidationException(result);
                }
            }
        }
    }
}