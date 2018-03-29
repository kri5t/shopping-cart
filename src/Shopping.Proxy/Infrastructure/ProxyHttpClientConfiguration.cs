using System;

namespace Shopping.Proxy.Infrastructure
{
    public interface IProxyHttpClientConfiguration
    {
        string ApiBaseUrl { get; }
        string ApiName { get; }
        TimeSpan Timeout { get; }
    }

    public class ProxyHttpClientConfiguration : IProxyHttpClientConfiguration
    {
        public string ApiBaseUrl { get; }
        public string ApiName { get; }
        public TimeSpan Timeout { get; }
    }
}