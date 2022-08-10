using System.Text.Json;

namespace Netcool.Caching;

public class SystemTextJsonSerializer : ISerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SystemTextJsonSerializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
    }

    public SystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();
    }

    public byte[] Serialize<T>(T obj)
    {
        return JsonSerializer.SerializeToUtf8Bytes(obj, _jsonSerializerOptions);
    }

    public T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes, _jsonSerializerOptions);
    }
}
