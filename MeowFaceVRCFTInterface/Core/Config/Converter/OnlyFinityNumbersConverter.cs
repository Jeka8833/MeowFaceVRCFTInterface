using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Core.Config.Converter;

public class OnlyFinityNumbersConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        Type type = Nullable.GetUnderlyingType(objectType) ?? objectType;

        return type == typeof(float) || type == typeof(double);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        Type? nullableBase = Nullable.GetUnderlyingType(objectType);

        if (nullableBase != null && reader.TokenType == JsonToken.Null) return null;

        Type type = nullableBase ?? objectType;

        object? textValue = reader.Value;
        if (textValue == null) return existingValue;

        if (type == typeof(float))
        {
            float value = Convert.ToSingle(textValue);

            return float.IsFinite(value) ? value : existingValue;
        }

        if (type == typeof(double))
        {
            double value = Convert.ToDouble(textValue);

            return double.IsFinite(value) ? value : existingValue;
        }

        return existingValue;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        switch (value)
        {
            case double d when double.IsFinite(d):
                writer.WriteValue(d);
                break;
            case float f when float.IsFinite(f):
                writer.WriteValue(f);
                break;
            default:
                writer.WriteNull(); // Write NaN, -Inf and Inf as Null, on next serialization will be set default value
                break;
        }
    }
}