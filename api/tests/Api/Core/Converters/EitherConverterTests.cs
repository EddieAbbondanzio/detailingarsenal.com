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
    public class EitherConverterTests {
        [TestMethod]
        public void FactoryCanConvertReturnsTrue() {
            var c = new EitherConverter();
            Assert.IsTrue(c.CanConvert(typeof(Either<int, string>)));
        }

        [TestMethod]
        public void FactoryCanConvertReturnsFalse() {
            var c = new EitherConverter();
            Assert.IsFalse(c.CanConvert(typeof(int)));
            Assert.IsFalse(c.CanConvert(typeof(string)));
            Assert.IsFalse(c.CanConvert(typeof(object)));
        }

        [TestMethod]
        public void CanConvertReturnsTrueForEithers() {
            var c = new EitherConverterInner<int, string>();
            Assert.IsTrue(c.CanConvert(typeof(Either<int, string>)));
        }

        [TestMethod]
        public void CanConvertReturnsFalseForEverythingElse() {
            var c = new EitherConverterInner<int, string>();
            var can = c.CanConvert(typeof(Boolean));

            Assert.IsFalse(c.CanConvert(typeof(object)));
            Assert.IsFalse(c.CanConvert(typeof(string)));
            Assert.IsFalse(c.CanConvert(typeof(decimal)));
            Assert.IsFalse(c.CanConvert(typeof(int)));
            Assert.IsFalse(can);

        }

        [TestMethod]
        public void ReadReadsLeft() {
            var c = new EitherConverterInner<int, string>();

            var rawBytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("\"cat\""));
            var reader = new Utf8JsonReader(rawBytes);

            reader.Read(); // Read always starts at none. See: https://stackoverflow.com/questions/59028159/exception-when-testing-custom-jsonconverter
            var output = c.Read(ref reader, typeof(Either<int, string>), null!);
            Assert.AreEqual(new("cat"), output);
        }

        [TestMethod]
        public void ReadReadsRight() {
            var c = new EitherConverterInner<int, string>();

            var rawBytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("1"));
            var reader = new Utf8JsonReader(rawBytes);

            reader.Read(); // Read always starts at none. See: https://stackoverflow.com/questions/59028159/exception-when-testing-custom-jsonconverter
            var output = c.Read(ref reader, typeof(Either<int, string>), null!);
            Assert.AreEqual(new(1), output);
        }

        [TestMethod]
        public void ReadReadsRightObject() {
            var c = new EitherConverterInner<int, Foo>();

            var rawBytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("{\"Bar\": \"cat\", \"Baz\": 1}"));
            var reader = new Utf8JsonReader(rawBytes);

            reader.Read(); // Read always starts at none. See: https://stackoverflow.com/questions/59028159/exception-when-testing-custom-jsonconverter
            var output = c.Read(ref reader, typeof(Either<int, Foo>), null!);
            Assert.AreEqual("cat", output!.Right().Bar);
            Assert.AreEqual(1, output!.Right().Baz);
        }


        [TestMethod]
        public void WriteWritesLeft() {
            var c = new EitherConverterInner<int, string>();

            using (var ms = new MemoryStream()) {
                using (var writer = new Utf8JsonWriter(ms)) {
                    c.Write(writer, new Either<int, string>(1), null!);

                    writer.Flush();

                    var output = Encoding.UTF8.GetString(ms.ToArray());

                    Assert.AreEqual("1", output);
                }
            }
        }

        [TestMethod]
        public void WriteWritesRight() {
            var c = new EitherConverterInner<int, string>();

            using (var ms = new MemoryStream()) {
                using (var writer = new Utf8JsonWriter(ms)) {
                    c.Write(writer, new Either<int, string>("cat"), null!);

                    writer.Flush();

                    var output = Encoding.UTF8.GetString(ms.ToArray());

                    Assert.AreEqual("\"cat\"", output);
                }
            }
        }

        class Foo {
            public string Bar { get; set; } = null!;
            public int Baz { get; set; }
        }
    }
}