using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public abstract class SagaStep {
        public abstract Task Execute();
        public abstract Task Compensate();
    }

    public abstract class SagaStep<TInput> {
        public abstract Task Execute(TInput input);
        public abstract Task Compensate(TInput input);
    }
}