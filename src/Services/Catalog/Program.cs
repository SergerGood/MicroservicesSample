using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;

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
    .AddMarten(options => options.Connection(builder.Configuration.GetConnectionString("Database")!))
    .UseLightweightSessions();

if (builder.Environment.IsDevelopment()) 
    builder.Services.InitializeMartenWith<CatalogInitialData>();

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(_ => { });

app.Run();