using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public interface IMediator {
    Task Dispatch<TInput>(TInput input, string userId = "") where TInput : class, IAction;
    Task<TOutput> Dispatch<TInput, TOutput>(TInput input, string userId = "") where TInput : class, IAction;
}

/// <summary>
/// Mediator to handle issuing commands, and queries.
/// </summary>
public sealed class Mediator : IMediator {
    #region Fields
    private IServiceProvider serviceProvider;
    #endregion

    #region Constructor(s)
    public Mediator(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;
    }
    #endregion

    public async Task Dispatch<TInput>(TInput input, string userId = "") where TInput : class, IAction {
        ActionHandler<TInput> handler = serviceProvider.GetRequiredService<ActionHandler<TInput>>();
        handler.Init(serviceProvider);

        await handler.Execute(input, userId);
    }

    public async Task<TOutput> Dispatch<TInput, TOutput>(TInput input, string userId = "") where TInput : class, IAction {
        ActionHandler<TInput, TOutput> handler = serviceProvider.GetRequiredService<ActionHandler<TInput, TOutput>>();
        handler.Init(serviceProvider);

        return await handler.Execute(input, userId);
    }
}