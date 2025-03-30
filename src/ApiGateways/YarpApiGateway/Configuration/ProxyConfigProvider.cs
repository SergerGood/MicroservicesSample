using Yarp.ReverseProxy.Configuration;

namespace YarpApiGateway.Configuration;

public class ProxyConfigProvider: IProxyConfigProvider
{
    public IProxyConfig GetConfig() => new ProxyConfig();
}