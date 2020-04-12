using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Converter that turns any date into MM/DD/YYY format. The default ISO 8601 format
/// is causing the front end to apply it's own time to it causing an off by 1 error.
/// </summary>
public class DateTimeJsonConverter : JsonConverter<DateTime> {
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return DateTime.ParseExact(reader.GetString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
    }
}