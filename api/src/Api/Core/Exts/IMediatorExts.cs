using System.Security.Claims;
using System.Threading.Tasks;
using DetailingArsenal.Application;

namespace DetailingArsenal.Api {
    /// <summary>
    /// Wedge to reduce boilerplate by getting rid of the need to call User.GetUser() again, and again, and again...
    /// </summary>
    public static class IMediatorExts {
        /// <summary>
        /// Dispatch a command or query that doesn't need any input.
        /// </summary>
        public async static Task Dispatch<TInput>(this IMediator mediator, ClaimsPrincipal? user = null) where TInput : class, IAction, new() {
            await mediator.Dispatch<TInput>(user?.GetUserId());
        }

        /// <summary>
        /// Dispatch a command or query that doesn't need an input but returns a value.
        /// </summary>
        public async static Task<TOutput> Dispatch<TInput, TOutput>(this IMediator mediator, ClaimsPrincipal? user = null) where TInput : class, IAction, new() {
            return await mediator.Dispatch<TInput, TOutput>(user?.GetUserId());
        }

        /// <summary>
        /// Dispatch a command or query that has no output.
        /// </summary>
        public async static Task Dispatch<TInput>(this IMediator mediator, TInput input, ClaimsPrincipal? user = null) where TInput : class, IAction {
            await mediator.Dispatch<TInput>(input, user?.GetUserId());
        }

        /// <summary>
        /// Dispatch a command or query that has an input, and return value.
        /// </summary>
        public async static Task<TOutput> Dispatch<TInput, TOutput>(this IMediator mediator, TInput input, ClaimsPrincipal? user = null) where TInput : class, IAction {
            return await mediator.Dispatch<TInput, TOutput>(input, user?.GetUserId());

        }
    }
}