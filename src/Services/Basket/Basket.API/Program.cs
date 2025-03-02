using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssembly(assembly);
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    })
    .AddValidatorsFromAssembly(assembly)
    .AddCarter();

builder.Services
    .AddMarten(options =>
    {
        options.Connection(GetConnectionString(builder));
        options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    })
    .UseLightweightSessions();

var app = builder.Build();

app.MapCarter();

app.Run();
return;

string GetConnectionString(WebApplicationBuilder webApplicationBuilder)
{
    return webApplicationBuilder.Configuration.GetConnectionString("Database")
           ?? throw new InvalidOperationException("Database connection string is missing");
}