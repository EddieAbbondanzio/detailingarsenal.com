using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Shared {
    [TestClass]
    public class EnumUtilsTests {
        [TestMethod]
        public void GetValues() {
            var values = EnumUtils.GetValues<Animal>();
            Assert.AreEqual(Animal.Cat, values.ElementAt(0));
            Assert.AreEqual(Animal.Dog, values.ElementAt(1));
            Assert.AreEqual(Animal.Horse, values.ElementAt(2));
        }

        [TestMethod]
        public void GetAllAttributesByValues() {
            var map = EnumUtils.GetAllAttributesByValues<Animal, JsonValueAttribute>();

            Assert.AreEqual(map.Count(), 3);
            Assert.AreEqual(Animal.Cat, map.ElementAt(0).Value);
            Assert.AreEqual(Animal.Dog, map.ElementAt(1).Value);
            Assert.AreEqual(Animal.Horse, map.ElementAt(2).Value);
            Assert.IsInstanceOfType(map.ElementAt(0).Attribute, typeof(JsonValueAttribute));
            Assert.IsInstanceOfType(map.ElementAt(1).Attribute, typeof(JsonValueAttribute));
            Assert.IsNull(map.ElementAt(2).Attribute);

        }

        enum Animal {
            [JsonValue("c")]
            Cat,
            [JsonValue("d")]
            Dog,
            Horse
        }
    }
}