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
    })
    .AddValidatorsFromAssembly(assembly)
    .AddCarter();

builder.Services
    .AddMarten(options => options.Connection(builder.Configuration.GetConnectionString("Database")!))
    .UseLightweightSessions();


var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(_ => { });

app.Run();