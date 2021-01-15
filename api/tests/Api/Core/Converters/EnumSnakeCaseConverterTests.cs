using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DetailingArsenal.Api;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Api {
    [TestClass]
    public class EnumSnakeCaseConverterTests {
        [TestMethod]
        public void CanConvertReturnsTrueForEnums() {
            var c = new EnumSnakeCaseConverter();
            Assert.IsTrue(c.CanConvert(typeof(SampleEnum)));
        }

        [TestMethod]
        public void CanConvertReturnsFalseForEverythingElse() {
            var c = new EnumSnakeCaseConverter();
            Assert.IsFalse(c.CanConvert(typeof(int)));
            Assert.IsFalse(c.CanConvert(typeof(object)));
            Assert.IsFalse(c.CanConvert(typeof(string)));
        }

        [TestMethod]
        public void ReadHandlesNull() {
            var c = new EnumSnakeCaseConverter();

            var rawBytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("null"));
            var reader = new Utf8JsonReader(rawBytes);

            reader.Read(); // Read always starts at none. See: https://stackoverflow.com/questions/59028159/exception-when-testing-custom-jsonconverter
            var e = c.Read(ref reader, typeof(SampleEnum), null!);
            Assert.AreEqual(null, e);
        }

        [TestMethod]
        public void ReadReturnsEnum() {
            var c = new EnumSnakeCaseConverter();

            var rawBytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("\"cat_dog\""));
            var reader = new Utf8JsonReader(rawBytes);

            reader.Read(); // Read always starts at none. See: https://stackoverflow.com/questions/59028159/exception-when-testing-custom-jsonconverter
            var e = c.Read(ref reader, typeof(SampleEnum), null!);
            Assert.AreEqual(SampleEnum.CatDog, e);
        }

        [TestMethod]
        public void WriteHanldesNull() {
            var c = new EnumSnakeCaseConverter();

            using (var ms = new MemoryStream()) {
                using (var writer = new Utf8JsonWriter(ms)) {
                    c.Write(writer, null!, null!);

                    writer.Flush();

                    var output = Encoding.UTF8.GetString(ms.ToArray());

                    Assert.AreEqual("null", output);
                }
            }
        }

        [TestMethod]
        public void WriteWritesEnum() {
            var c = new EnumSnakeCaseConverter();

            using (var ms = new MemoryStream()) {
                using (var writer = new Utf8JsonWriter(ms)) {
                    c.Write(writer, SampleEnum.CatDog, null!);

                    writer.Flush();

                    var output = Encoding.UTF8.GetString(ms.ToArray());

                    Assert.AreEqual("\"cat_dog\"", output);
                }
            }
        }

        enum SampleEnum {
            CatDog
        }
    }
}