using Yarp.ReverseProxy.Configuration;
using YarpApiGateway;
using YarpApiGateway.Configuration;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddTransient<IProxyConfigProvider, ProxyConfigProvider>();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetRequiredSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();