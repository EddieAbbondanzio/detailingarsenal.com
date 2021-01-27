using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Api {

    public class EitherConverter : JsonConverterFactory {
        public override bool CanConvert(Type typeToConvert) {
            return IsSubclassOfRawGeneric(typeof(Either<,>), typeToConvert);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
            // See: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#sample-factory-pattern-converter

            var generics = typeToConvert.GetGenericArguments();
            var leftType = generics[0];
            var rightType = generics[1];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(EitherConverterInner<,>).MakeGenericType(new Type[] { leftType, rightType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null, // These must match constructor on EnumSnakeCaseConverterInner<>. Currently using the empty constructor.
                culture: null)!;

            return converter;
        }

        bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType!;
            }
            return false;
        }
    }


    public class EitherConverterInner<TLeft, TRight> : JsonConverter<Either<TLeft, TRight>> {
        public override Either<TLeft, TRight>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            /*
            * While it may be a bit of a performance hit, we'll simply try to deserialize the left type first,
            * and if that fails, then we'll try the right side.
            */
            try {
                var l = JsonSerializer.Deserialize<TLeft>(ref reader, options)!;
                return new(l);
            } catch (JsonException) { }

            try {
                var r = JsonSerializer.Deserialize<TRight>(ref reader, options)!;
                return new(r);
            } catch (JsonException) { }

            throw new InvalidOperationException($"Failed to deserialize a {typeof(TLeft).Name} or {typeof(TRight).Name}");
        }

        public override void Write(Utf8JsonWriter writer, Either<TLeft, TRight> value, JsonSerializerOptions options) {
            if (value.IsLeft) {
                JsonSerializer.Serialize(writer, value.Left(), options);
            } else {
                JsonSerializer.Serialize(writer, value.Right(), options);
            }
        }
    }
}