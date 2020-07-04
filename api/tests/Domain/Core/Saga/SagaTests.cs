using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain {
    [TestClass]
    public class SagaTests {
        [TestMethod]
        public async Task ExecuteCallsEveryStep() {
            var stepA = new Mock<SagaStep>();
            var stepB = new Mock<SagaStep>();

            var saga = Mock.Of<Saga>();
            saga.Add(stepA.Object);
            saga.Add(stepB.Object);

            await saga.Execute();

            stepA.Verify(s => s.Execute(It.IsAny<SagaContext>()), Times.Once);
            stepB.Verify(s => s.Execute(It.IsAny<SagaContext>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteCompensatesOnException() {
            var stepA = new Mock<SagaStep>();
            var stepB = new Mock<SagaStep>();
            stepB.Setup(s => s.Execute(It.IsAny<SagaContext>())).ThrowsAsync(new Exception());

            var saga = Mock.Of<Saga>();
            saga.Add(stepA.Object);
            saga.Add(stepB.Object);

            try {
                await saga.Execute();
            } catch { }

            stepA.Verify(s => s.Compensate(It.IsAny<SagaContext>()), Times.Once);
        }
    }
}
