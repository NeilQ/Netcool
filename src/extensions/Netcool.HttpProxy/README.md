# Netcool.HttpProxy

A http proxy for asp.net core app.  

Most of the codes comes from [aspnet/AspLabs](https://github.com/aspnet/AspLabs)

## Usage

### Configure services
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddProxy(options =>
    {
        options.MessageHandler = new HttpClientHandler
        {
            AllowAutoRedirect = false
        };
        
        options.PrepareRequest = (originalRequest, message) =>
        {
            message.Headers.Add("X-Forwarded-Host", originalRequest.Host.Host);
            return Task.FromResult(0);
        };
    });
}
```

### Map proxy
```c#
// map with applicationBuilder
 app.Map("/baidu", builder => { builder.RunProxy("https://www.baidu.com"); });
 app.MapProxy("/baiduIp", "https://www.baidu.com/s?wd=ip");
 
// map proxy with endpoint
 app.UseEndpoints(endpoints =>
 {
     endpoints.MapProxy("/baidu2/s/{wd:alpha}", dictionary => $"https://www.baidu.com/s?wd={dictionary["wd"]}");
     endpoints.MapProxy("/baidu3", _ => "https://www.baidu.com/s");
 });

```

