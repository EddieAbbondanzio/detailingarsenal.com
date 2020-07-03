using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public abstract class Saga {
        public List<SagaStep> Steps = null!;

        public async Task Execute() {
            int i = 0;
            SagaContext context = new SagaContext();

            try {
                for (; i < Steps.Count; i++) {
                    await Steps[i].Execute(context);
                }
            } catch {
                for (i -= 1; i >= 0; i--) {
                    await Steps[i].Compensate(context);
                }

                throw;
            }
        }

        public void Add(SagaStep step) {
            if (Steps == null) {
                Steps = new List<SagaStep>();
            }

            Steps.Add(step);
        }
    }

    public abstract class Saga<TInput> {
        public List<SagaStep<TInput>> Steps = null!;

        public async Task Execute(TInput input) {
            int i = 0;
            SagaContext<TInput> context = new SagaContext<TInput>(input);

            try {
                for (; i < Steps.Count; i++) {
                    await Steps[i].Execute(context);
                }
            } catch {
                for (i -= 1; i >= 0; i--) {
                    await Steps[i].Compensate(context);
                }

                throw;
            }
        }

        public void Add(SagaStep<TInput> step) {
            if (Steps == null) {
                Steps = new List<SagaStep<TInput>>();
            }

            Steps.Add(step);
        }
    }
}