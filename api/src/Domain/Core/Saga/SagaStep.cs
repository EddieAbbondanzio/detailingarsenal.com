using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public abstract class SagaStep {
        public abstract Task Execute(SagaContext context);
#pragma warning disable 1998
        public async virtual Task Compensate(SagaContext context) { }
#pragma warning restore 1998
    }

    public abstract class SagaStep<TInput> {
        public abstract Task Execute(SagaContext<TInput> context);
#pragma warning disable 1998
        public async virtual Task Compensate(SagaContext<TInput> context) { }
#pragma warning restore 1998
    }
}