namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services) => services;

    public static WebApplication UseApiServices(this WebApplication app) => app;
}