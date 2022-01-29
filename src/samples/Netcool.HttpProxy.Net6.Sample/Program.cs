using Microsoft.AspNetCore.Builder;
using Netcool.HttpProxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject proxy
builder.Services.AddProxy(options => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseRouting();
app.MapProxy("/baiduIp", "https://www.baidu.com/s?wd=ip");
app.Map("/baidu", appBuilder => { appBuilder.RunProxy("https://www.baidu.com/"); });

// map proxy with endpoint
app.MapProxy("/baidu2/s/{wd:alpha}", dictionary => $"https://www.baidu.com/s?wd={dictionary["wd"]}").WithName("");
app.MapProxy("/baidu3", _ => "https://www.baidu.com/s");


app.Run();
