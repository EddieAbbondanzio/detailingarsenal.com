using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application.Users;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DetailingArsenal.Application {
    public interface IMediator {
        Task Dispatch<TInput>(TInput input, string? userId = null) where TInput : class, IAction;
        Task<TOutput> Dispatch<TInput, TOutput>(TInput input, string? userId = null) where TInput : class, IAction;
    }

    /// <summary>
    /// Mediator to handle issuing commands, and queries.
    /// </summary>
    public sealed class Mediator : IMediator {
        IServiceProvider serviceProvider;
        IEnumerable<ActionMiddleware> middlewares;
        IUserResolver userResolver;

        public Mediator(IServiceProvider serviceProvider, IEnumerable<ActionMiddleware> middlewares) {
            this.serviceProvider = serviceProvider;
            this.middlewares = middlewares;

            userResolver = serviceProvider.GetRequiredService<IUserResolver>();
        }

        public async Task Dispatch<TInput>(TInput input, string? userId = null) where TInput : class, IAction {
            ActionHandler<TInput> handler = serviceProvider.GetRequiredService<ActionHandler<TInput>>();

            var user = userId != null ? await userResolver.Resolve(userId) : null;

            foreach (ActionMiddleware mw in middlewares) {
                await mw.Execute(serviceProvider, handler, input, user);
            }

            await handler.Execute(input, user);
        }

        public async Task<TOutput> Dispatch<TInput, TOutput>(TInput input, string? userId = null) where TInput : class, IAction {
            ActionHandler<TInput, TOutput> handler = serviceProvider.GetRequiredService<ActionHandler<TInput, TOutput>>();

            var user = userId != null ? await userResolver.Resolve(userId) : null;

            foreach (ActionMiddleware mw in middlewares) {
                await mw.Execute(serviceProvider, handler, input, user);
            }

            return await handler.Execute(input, user);
        }
    }
}