using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var configuration = builder.Configuration;

builder.Services
    .AddMediatR(options =>
    {
        options.RegisterServicesFromAssembly(assembly);
        options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        options.AddOpenBehavior(typeof(LoggingBehavior<,>));
    })
    .AddCarter();

builder.Services
    .AddScoped<IBasketRepository, BasketRepository>()
    .Decorate<IBasketRepository, CachedBasketRepository>()
    .AddMarten(options =>
    {
        options.Connection(GetDbConnectionString(configuration));
        options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    })
    .UseLightweightSessions();

builder.Services
    .AddStackExchangeRedisCache(options => options.Configuration = GetRedisConnectionString(configuration));

builder.Services
    .AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    {
        options.Address = new Uri(GetDiscountAddress(configuration));
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>()
    .AddValidatorsFromAssembly(assembly);

builder.Services
    .AddHealthChecks()
    .AddNpgSql(GetDbConnectionString(configuration))
    .AddRedis(GetRedisConnectionString(configuration));

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

string GetDbConnectionString(ConfigurationManager configurationManager)
{
    return configurationManager.GetConnectionString("Database")
           ?? throw new InvalidOperationException("Database connection string is missing");
}

string GetRedisConnectionString(ConfigurationManager configurationManager)
{
    return configurationManager.GetConnectionString("Redis")
           ?? throw new InvalidOperationException("Redis connection string is missing");
}

string GetDiscountAddress(ConfigurationManager configurationManager)
{
    return configurationManager["GrpcSettings:DiscountAddress"]
           ?? throw new InvalidOperationException("Discount address is missing");
}