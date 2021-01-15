using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Api {
    public class EnumSnakeCaseConverter : JsonConverter<Enum> {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override Enum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var raw = reader.GetString();

            if (raw == null) {
                return null;
            }

            var e = Enum.Parse(typeToConvert, StringUtils.ToPascalCaseFromSnake(raw)!);
            return e as Enum;
        }

        public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options) {
            if (value == null) {
                writer.WriteNullValue();
                return;
            }

            var og = Enum.GetName(value.GetType(), value);

            if (og == null) {
                writer.WriteNullValue();
            } else {
                writer.WriteStringValue(StringUtils.ToSnakeCaseFromPascal(og));
            }
        }
    }
}