using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            if (assembly is not null)
            {
                configurator.AddConsumers(assembly);
            }

            configurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(new Uri(configuration["RabbitMq:Host"]), hostConfigurator =>
                {
                    hostConfigurator.Username(configuration["RabbitMq:UserName"]);
                    hostConfigurator.Password(configuration["RabbitMq:Password"]);
                });
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}