using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netcool.Core.AspNetCore.Json;

/// <summary>
/// Deserialize json null value to int default value, as the net8 version of System.Text.Json does not support this.
/// </summary>
public class Int32Converter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.ValueSpan == Span<byte>.Empty) return 0;
        int.TryParse(reader.ValueSpan, out var value);
        return value;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
