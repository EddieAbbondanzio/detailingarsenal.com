using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Shared {
    [TestClass]
    public class StringUtilsTests {
        [TestMethod]
        public void UpperCaseFirstChar() {
            Assert.AreEqual("Cat", StringUtils.UpperCaseFirstChar("cat"));
        }

        [TestMethod]
        public void LowerCaseFirstChar() {
            Assert.AreEqual("cAT", StringUtils.LowerCaseFirstChar("CAT"));
        }

        [TestMethod]
        public void ToSnakeCaseHandles1Word() {
            Assert.AreEqual("cat", StringUtils.ToSnakeCaseFromPascal("Cat"));
        }

        [TestMethod]
        public void ToSnakeCaseHandlesMultipleWords() {
            Assert.AreEqual("cat_dog", StringUtils.ToSnakeCaseFromPascal("CatDog"));
        }

        [TestMethod]
        public void ToPascalCaseHandles1Word() {
            Assert.AreEqual("Cat", StringUtils.ToPascalCaseFromSnake("cat"));
        }

        [TestMethod]
        public void ToPascalCaseHandlesMultipleWords() {
            Assert.AreEqual("CatDog", StringUtils.ToPascalCaseFromSnake("cat_dog"));
        }
    }
}