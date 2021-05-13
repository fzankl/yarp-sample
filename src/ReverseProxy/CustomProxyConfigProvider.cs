using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Abstractions;
using Yarp.ReverseProxy.Abstractions.ClusterDiscovery.Contract;
using Yarp.ReverseProxy.Abstractions.Config;
using Yarp.ReverseProxy.Service;

namespace ReverseProxy
{
    public class CustomProxyConfigProvider : IProxyConfigProvider
    {
        private CustomMemoryConfig _config;

        public CustomProxyConfigProvider()
        {
            // Load a basic configuration
            // Should be based on your application needs.
            var route = new ProxyRoute
            {
                RouteId = "route1",
                ClusterId = "cluster1",
                Match = new RouteMatch
                {
                    Path = "/api/service1/{**catch-all}"
                }
            };

            route.WithTransformPathRemovePrefix(prefix: "/api/service1/");
            route.WithTransformResponseHeader(headerName: "Source", value: "YARP", append: true, always: false);

            var routes = new[] { route };

            var clusters = new[]
            {
                new Cluster
                {
                    Id = "cluster1",
                    LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                    Destinations = new Dictionary<string, Destination>
                    {
                        { "destination1", new Destination { Address = "https://localhost:5001/" } },
                        { "destination2", new Destination { Address = "https://localhost:5002/" } }
                    }
                }
            };

            _config = new CustomMemoryConfig(routes, clusters);
        }

        public IProxyConfig GetConfig() => _config;

        /// <summary>
        /// By calling this method from the source we can dynamically adjust the proxy configuration.
        /// Since our provider is registered in DI mechanism it can be injected via constructors anywhere.
        /// </summary>
        public void Update(IReadOnlyList<ProxyRoute> routes, IReadOnlyList<Cluster> clusters)
        {
            var oldConfig = _config;
            _config = new CustomMemoryConfig(routes, clusters);
            oldConfig.SignalChange();
        }

        private class CustomMemoryConfig : IProxyConfig
        {
            private readonly CancellationTokenSource _cts = new CancellationTokenSource();

            public CustomMemoryConfig(IReadOnlyList<ProxyRoute> routes, IReadOnlyList<Cluster> clusters)
            {
                Routes = routes;
                Clusters = clusters;
                ChangeToken = new CancellationChangeToken(_cts.Token);
            }

            public IReadOnlyList<ProxyRoute> Routes { get; }

            public IReadOnlyList<Cluster> Clusters { get; }

            public IChangeToken ChangeToken { get; }

            internal void SignalChange()
            {
                _cts.Cancel();
            }
        }
    }
}
