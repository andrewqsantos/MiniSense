using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiniSense.API.Converters;

public class GuidNoHyphensConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Guid.ParseExact(value!, "N");
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("N"));
    }
}