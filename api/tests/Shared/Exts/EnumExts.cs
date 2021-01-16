using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Shared {
    [TestClass]
    public class EnumExts {
        [TestMethod]
        public void GetCustomAttribute() {
            var te = TestEnum.Value;
            var ta = te.GetCustomAttribute<TestAttribute>();

            Assert.IsInstanceOfType(ta, typeof(TestAttribute));
        }

        enum TestEnum {
            [Test]
            Value
        }

        class TestAttribute : Attribute { }
    }
}