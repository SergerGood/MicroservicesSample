using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddTransient<IProxyConfigProvider, ProxyConfigProvider>();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetRequiredSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.PermitLimit = 5;
    });
});

var app = builder.Build();

app.UseRateLimiter();
app.MapReverseProxy();

app.Run();