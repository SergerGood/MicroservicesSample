using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace YarpApiGateway.Configuration;

public class ProxyConfig : IProxyConfig
{
    private static readonly string clusterId = "cluster1";
    private static readonly CancellationTokenSource cts = new();

    public IReadOnlyList<RouteConfig> Routes { get; } = GenerateRoutes();

    public IReadOnlyList<ClusterConfig> Clusters { get; } = GenerateClusters();

    public IChangeToken ChangeToken { get; } = new CancellationChangeToken(cts.Token);

    private static IReadOnlyList<RouteConfig> GenerateRoutes()
    {
        return new List<RouteConfig>
        {
            new()
            {
                RouteId = "route1",
                ClusterId = clusterId,
                Match = new RouteMatch { Path = "{**catch-all}" }
            }
        };
    }

    private static IReadOnlyList<ClusterConfig> GenerateClusters()
    {
        return new List<ClusterConfig>
        {
            new()
            {
                ClusterId = clusterId,
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        "destination1", new DestinationConfig
                        {
                            Address = "http://localhost:6000/products"
                        }
                    }
                }
            }
        };
    }
}