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
            Assert.IsTrue(c.CanConvert(typeof(Cartoon)));
        }

        [TestMethod]
        public void CanConvertReturnsFalseForEverythingElse() {
            var c = new EnumSnakeCaseConverter();
            Assert.IsFalse(c.CanConvert(typeof(int)));
            Assert.IsFalse(c.CanConvert(typeof(object)));
            Assert.IsFalse(c.CanConvert(typeof(string)));
        }

        [TestMethod]
        public void CanConvertReturnsFalseForFlags() {
            var c = new EnumSnakeCaseConverter();
            Assert.IsFalse(c.CanConvert(typeof(FlagEnum)));
        }

        [TestMethod]
        public void CreateConverterReturnsValidInstance() {
            var converter = new EnumSnakeCaseConverter().CreateConverter(typeof(Cartoon), null!);

            Assert.IsNotNull(converter);
            Assert.IsTrue(converter!.CanConvert(typeof(Cartoon)));
            Assert.IsFalse(converter!.CanConvert(typeof(int)));
        }


        [TestMethod]
        public void ReadReturnsEnum() {
            var fc = new EnumSnakeCaseConverter();
            var c = fc.CreateConverter(typeof(Cartoon), null!) as EnumSnakeCaseConverterInner<Cartoon>;

            var rawBytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("\"cat_dog\""));
            var reader = new Utf8JsonReader(rawBytes);

            reader.Read(); // Read always starts at none. See: https://stackoverflow.com/questions/59028159/exception-when-testing-custom-jsonconverter
            var e = c!.Read(ref reader, typeof(Cartoon), null!);
            Assert.AreEqual(Cartoon.CatDog, e);
        }

        [TestMethod]
        public void WriteWritesEnumValueAsSnakeCase() {
            var c = new EnumSnakeCaseConverterInner<Cartoon>();

            using (var ms = new MemoryStream()) {
                using (var writer = new Utf8JsonWriter(ms)) {
                    c.Write(writer, Cartoon.CatDog, null!);

                    writer.Flush();

                    var output = Encoding.UTF8.GetString(ms.ToArray());

                    Assert.AreEqual("\"cat_dog\"", output);
                }
            }
        }

        [TestMethod]
        public void WriteWritesValueFromAttribute() {
            var c = new EnumSnakeCaseConverterInner<Cartoon>();

            using (var ms = new MemoryStream()) {
                using (var writer = new Utf8JsonWriter(ms)) {
                    c.Write(writer, Cartoon.BojackHorseman, null!);

                    writer.Flush();

                    var output = Encoding.UTF8.GetString(ms.ToArray());

                    Assert.AreEqual("\"that_horse_from_horsin_around\"", output);
                }
            }
        }

        enum Cartoon {
            CatDog,
            [JsonValue("that_horse_from_horsin_around")]
            BojackHorseman
        }

        [Flags]
        enum FlagEnum {
            Foo = 1,
            Bar = 2
        }
    }
}