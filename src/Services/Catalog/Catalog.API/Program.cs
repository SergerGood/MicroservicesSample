using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
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
    .AddMarten(options => options.Connection(GetConnectionString(builder)))
    .UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(GetConnectionString(builder));

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

string GetConnectionString(WebApplicationBuilder webApplicationBuilder)
{
    return webApplicationBuilder.Configuration.GetConnectionString("Database")
           ?? throw new InvalidOperationException("Database connection string is missing");
}