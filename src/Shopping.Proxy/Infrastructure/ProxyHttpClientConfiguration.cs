using System;

namespace Shopping.Proxy.Infrastructure
{

    public class ProxyHttpClientConfiguration
    {
        public string ApiBaseUrl => "http://localhost:5000";
        public string ApiName => "Shopping cart proxy";
        public TimeSpan Timeout => TimeSpan.FromSeconds(10);
    }
}