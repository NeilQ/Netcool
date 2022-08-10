# Netcool.Caching
[![GitHub](https://img.shields.io/github/license/neilq/Netcool)](https://github.com/NeilQ/Netcool/blob/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Netcool.Caching)](https://www.nuget.org/packages/Netcool.Caching/)
![Nuget](https://img.shields.io/nuget/dt/Netcool.Caching)

Netcool.Caching offers enhanced extensions for [Distributed caching in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed), both MemoryDistributeCache and RedisCache.

## Configuration

### Distributed Redis cache
```c#
builder.Services.AddNetcoolRedisCache(options =>
{
    options.Configuration = "localhost";
});
```

### Distributed Memory Cache
```c#
builder.Services.AddNetcoolDistributedMemoryCache();
```

### Use in application service
```c#
public class MyService : IService
{
    private readonly INetcoolDistributedCache _cache;

    public MyService(INetcoolDistributedCache cache)
    {
        _cache = cache;
    }

    public void SetCache()
    {
        var obj = new WeatherForecast
        {
            Date = DateTime.Now,
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };
        _cache.SetObject("WeatherForecast", obj,
            new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5) });
    }
}
```


### Extensions

### Set and get object
```c#
cache.SetObject("Person", new Person());
var person = cache.GetObject("Person");

await cache.SetObjectAsync("Person", new Person());
var person =await cache.GetObjectAsync("Person");
```

### Increase and decrease
```c#
cache.SetString("count", "1");

var curr = cache.Increase("count",byValue:2, maxValue:10);
var curr2 = await cache.IncreaseAsync("count",2);

var res = cache.GetString("count");
Assert.AreEqual("3", res);
Assert.AreEqual(3, curr);
```

```c#
cache.SetString("count", "1");

var curr = cache.Decrease("count", byValue:2, minValue:1);
var curr2 = await cache.DecreaseAsync("count", byValue:2, minValue:1);

var res = cache.GetString("count");
Assert.AreEqual(1, curr);
Assert.AreEqual("1", res);
```

### Object Serializer

The default object serializer is SystemTextJsonSerializer, you can change the [JsonSerializerOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-6.0).
```c#
builder.Services.AddNetcoolRedisCache(options =>
{
    options.Configuration = "localhost";
    options.ObjectSerializer = new SystemTextJsonSerializer(new JsonSerializerOptions());
});
```


Otherwise, you can customize your own serializer:
```c#
public class MySerializer : ISerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public byte[] Serialize<T>(T obj)
    {
        return Encoding.UTF8.GetBytes(obj.ToString());
    }

    public T Deserialize<T>(byte[] bytes)
    {
        return (T)Convert.ChangeType(Encoding.UTF8.GetString(b).ToString(), typeof(T));
    }
}
```

Then configure at startup:

```c#
builder.Services.AddNetcoolRedisCache(options =>
{
    options.Configuration = "localhost";
    options.ObjectSerializer = new MySerializer();
});
```
