# Netcool.HttpProxy

A http proxy for asp.net core app base on netstandard2.1.

Most of the codes comes from [aspnet/AspLabs](https://github.com/aspnet/AspLabs)

## Usage

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

public void Configure(IApplicationBuilder app)
{
    app.Map("/api", builder => { builder.RunProxy(new Uri("http://api.domain.com")); });
}

```

