using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Api {
    /*
    * This complexity is needed. We need to create a factory that can produce concrete implementations of converters for each and
    * every enum. If we don't, our converter is given typeToConvert = System.Enum as a parameter to Read()
    * which will throw an exception when we call Enum.Parse(). 
    * 
    * It's something to do with a runtime issue or some other mumbo jumbo.
    *
    */

    public class EnumSnakeCaseConverter : JsonConverterFactory {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
            // See: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#sample-factory-pattern-converter

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(EnumSnakeCaseConverterInner<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null, // These must match constructor on EnumSnakeCaseConverterInner<>. Currently using the empty constructor.
                culture: null)!;

            return converter;
        }
    }

    public class EnumSnakeCaseConverterInner<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum {
        Dictionary<string, TEnum>? cache = null;

        Dictionary<string, TEnum> GetMap() {
            if (cache == null) {
                cache = new();

                var attributesByValue = EnumUtils.GetAllAttributesByValues<TEnum, JsonValueAttribute>();

                foreach (var (v, a) in attributesByValue) {
                    cache.Add(a?.Value ?? Enum.GetName(v.GetType(), v)!, v);
                }
            }

            return cache;
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var map = GetMap();

            var rawValue = reader.GetString()!;
            TEnum value;

            // Check for custom value from json attribute.
            if (map.TryGetValue(rawValue, out value)) {
                return value;
            }

            // Check for raw value serialized in snake case.
            if (map.TryGetValue(StringUtils.ToPascalCaseFromSnake(rawValue), out value)) {
                return value;
            }

            throw new ArgumentException($"No value: {rawValue} found on enum {typeToConvert.Name}");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) {
            var enumValue = Enum.GetName(value.GetType(), value); // Ex: Animal.Dog => "Dog"

            if (enumValue == null) {
                writer.WriteNullValue();
                return;
            }

            var jsonValueAttribute = value.GetCustomAttribute<JsonValueAttribute>();
            var jsonValue = jsonValueAttribute?.Value ?? StringUtils.ToSnakeCaseFromPascal(enumValue);

            writer.WriteStringValue(jsonValue);
        }
    }
}