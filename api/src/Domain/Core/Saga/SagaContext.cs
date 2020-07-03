using System.Dynamic;

namespace DetailingArsenal.Domain {
    public class SagaContext {
        public dynamic Data { get; }

        public SagaContext() {
            Data = new ExpandoObject();
        }
    }

    public class SagaContext<TInput> : SagaContext {
        public TInput Input { get; }

        public SagaContext(TInput input) {
            Input = input;
        }
    }
}