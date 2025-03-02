using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;

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
        options.Connection(GetConnectionString(builder));
        options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    })
    .UseLightweightSessions();

builder.Services
    .Decorate<IBasketRepository, CachedBasketRepository>()
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(_ => { });

app.Run();
return;

string GetConnectionString(WebApplicationBuilder webApplicationBuilder)
{
    return webApplicationBuilder.Configuration.GetConnectionString("Database")
           ?? throw new InvalidOperationException("Database connection string is missing");
}