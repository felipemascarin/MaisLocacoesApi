using System.Globalization;
using System.Text.Json;

namespace MaisLocacoes.WebApi.Utils.Middlewares
{
    public class DateTimeConverterUsingUtc : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && reader.GetString() is string dateString && !string.IsNullOrWhiteSpace(dateString))
            {
                if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return DateTime.SpecifyKind(date, DateTimeKind.Utc);
                }
                else
                {
                    throw new JsonException("Formato de data inválido");
                }
            }

            throw new JsonException("Formato de data inválido");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        }
    }
}