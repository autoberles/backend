using System.Text.Json.Serialization;
using System.Text.Json;

namespace autoberles_backend.Classes
{
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly string format = "yyyy-MM-dd";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return string.IsNullOrEmpty(value) ? null : DateTime.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString(format));
            else
                writer.WriteNullValue();
        }
    }
}
