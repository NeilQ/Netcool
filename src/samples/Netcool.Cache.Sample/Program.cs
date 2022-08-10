using System.Text.Json;
using Netcool.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNetcoolRedisCache(options =>
{
    options.Configuration = "localhost";
    options.ObjectSerializer = new SystemTextJsonSerializer(new JsonSerializerOptions());
});

/*
builder.Services.AddNetcoolDistributedMemoryCache();
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
