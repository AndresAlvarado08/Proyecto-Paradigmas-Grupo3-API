using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ConvertDateOnly : JsonConverter<DateOnly>
{
    private readonly string _serializationFormat;

    public ConvertDateOnly() : this("yyyy-MM-dd") { }

    public ConvertDateOnly(string serializationFormat)
    {
        _serializationFormat = serializationFormat;
    }

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (DateOnly.TryParse(value, out var date))
            return date;

        throw new JsonException($"No se pudo convertir '{value}' a DateOnly.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_serializationFormat));
    }
}
