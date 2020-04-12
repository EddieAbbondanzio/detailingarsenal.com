using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public abstract class ActionHandler {
    public IValidator? Validator { get; private set; }
    protected IUserService userService = null!;

    public void Init(IServiceProvider provider) {
        var type = GetType();

        ValidateAttribute? validateAttribute = type.GetCustomAttribute<ValidateAttribute>(true);

        if (validateAttribute != null) {
            Validator = provider.GetRequiredService(validateAttribute.Validator) as IValidator;
        }

        userService = provider.GetRequiredService<IUserService>();
    }
}

public abstract class ActionHandler<TInput> : ActionHandler where TInput : class, IAction {
    public async Task Execute(TInput input, string? auth0Id) {
        // Check to see if input is valid first.
        if (Validator != null) {
            var result = await Validator.ValidateAsync(input);

            if (!result.IsValid) {
                throw new ValidationException(result);
            }
        }

        var user = auth0Id != null ? await userService.GetOrCreateUserByAuth0Id(auth0Id) : null;

        await Execute(input, user);
    }

    protected abstract Task Execute(TInput input, User? user);
}

public abstract class ActionHandler<TInput, TOutput> : ActionHandler where TInput : class, IAction {
    public async Task<TOutput> Execute(TInput input, string? auth0Id) {
        if (Validator != null) {
            var result = await Validator.ValidateAsync(input);

            if (!result.IsValid) {
                throw new ValidationException(result);
            }
        }

        var user = auth0Id != null ? await userService.GetOrCreateUserByAuth0Id(auth0Id) : null;

        return await Execute(input, user);
    }

    protected abstract Task<TOutput> Execute(TInput input, User? user);
}
