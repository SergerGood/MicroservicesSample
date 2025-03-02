using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>()
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssembly(assembly);
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    })
    .AddValidatorsFromAssembly(assembly)
    .AddCarter();

builder.Services
    .AddScoped<IBasketRepository, BasketRepository>()
    .AddMarten(options =>
    {
        options.Connection(GetDbConnectionString(builder));
        options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    })
    .UseLightweightSessions();

builder.Services
    .Decorate<IBasketRepository, CachedBasketRepository>()
    .AddStackExchangeRedisCache(options => options.Configuration = GetRedisConnectionString(builder));

builder.Services
    .AddHealthChecks()
    .AddNpgSql(GetDbConnectionString(builder))
    .AddRedis(GetRedisConnectionString(builder));

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(_ => { });
app.UseHealthChecks("/hc",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
return;

string GetDbConnectionString(WebApplicationBuilder webApplicationBuilder)
{
    return webApplicationBuilder.Configuration.GetConnectionString("Database")
           ?? throw new InvalidOperationException("Database connection string is missing");
}

string GetRedisConnectionString(WebApplicationBuilder webApplicationBuilder)
{
    return webApplicationBuilder.Configuration.GetConnectionString("Redis")
           ?? throw new InvalidOperationException("Redis connection string is missing");
}