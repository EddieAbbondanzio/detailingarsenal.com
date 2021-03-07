using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Shared {
    [TestClass]
    public class DependencyInjectionRegistrarTests {
        [TestMethod]
        public void FindAllDependenciesFindsAttributes() {
            IEnumerable<Assembly> thisAssembly = new[] { GetType().Assembly };
            var matches = DependencyInjectionRegistrar.FindAllDependencies(thisAssembly);

            Assert.IsTrue(matches.Count() == 2);
            Assert.AreEqual("TestDependency", matches.ElementAt(0).Type.Name);
            Assert.AreEqual("TestDependency2", matches.ElementAt(1).Type.Name);
        }

        [TestMethod]
        public void FindAllMiddlewaresFindsAll() {
            var mws = DependencyInjectionRegistrar.FindAllMiddlewares(new[] { GetType().Assembly });
            Assert.AreEqual(1, mws.Count());
        }

        [TestMethod]
        public void AddDependenciesCallsMiddleware() {
            IEnumerable<Assembly> thisAssembly = new[] { GetType().Assembly };
            System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            var mockMiddleware = new Mock<DependencyInjectionMiddleware>();

            var dependencies = new DependencyInjectionEntry[] {
                new (typeof(TestDependency), new DependencyInjectionAttribute()),
                new (typeof(TestDependency2), new DependencyInjectionAttribute()),
            };

            DependencyInjectionRegistrar.AddDependencies(
                Mock.Of<IServiceCollection>(),
                dependencies,
                new[] { mockMiddleware.Object as DependencyInjectionMiddleware }
            );

            mockMiddleware.Verify(m => m.BeforeEach(It.IsAny<DependencyInjectionEntry>()), Times.Exactly(dependencies.Count()));

        }

        [TestMethod]
        public void AddDependenciesThrowsIfInvalidRegisterAsType() {
            IEnumerable<Assembly> thisAssembly = new[] { GetType().Assembly };
            System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            Assert.ThrowsException<NotSupportedException>(() => {
                DependencyInjectionRegistrar.AddDependencies(Mock.Of<IServiceCollection>(), new DependencyInjectionEntry[] {
                new (typeof(TestDependency), new DependencyInjectionAttribute() { RegisterAs = typeof(string)})
            }, new DependencyInjectionMiddleware[] { });
            });
        }
    }

    [DependencyInjection]
    public class TestDependency { }

    [DependencyInjection]
    public class TestDependency2 { }


    public class FooMiddleware : DependencyInjectionMiddleware {
        public override void BeforeEach(DependencyInjectionEntry dependecy) {
            throw new NotImplementedException();
        }
    }
}