using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public abstract class SagaStep {
        public abstract Task Execute();
#pragma warning disable 1998
        public async virtual Task Compensate() { }
#pragma warning restore 1998
    }

    public abstract class SagaStep<TInput> {
        public abstract Task Execute(TInput input);
#pragma warning disable 1998
        public async virtual Task Compensate(TInput input) { }
#pragma warning restore 1998
    }
}