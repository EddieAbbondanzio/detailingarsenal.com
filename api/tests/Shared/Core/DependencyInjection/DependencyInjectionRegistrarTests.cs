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

            Assert.IsTrue(matches.Count() == 1);
            Assert.AreEqual("TestDependency", matches.ElementAt(0).Item1.Name);
        }
    }

    [DependencyInjection]
    public class TestDependency {

    }
}