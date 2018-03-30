using Microsoft.Extensions.DependencyInjection;
using Shopping.Proxy.Infrastructure;

namespace Shopping.Proxy
{
    public static class Installer
    {
        public static IServiceCollection InstallShoppingProxy(this IServiceCollection services)
        {
            services.AddTransient<IShoppingCartProxy, ShoppingCartProxy>();
            services.AddTransient<IItemProxy, ItemProxy>();
            services.AddTransient<ProxyHttpClientConfiguration>();
            services.AddTransient<ProxyHttpClient>();
            return services;
        } 
    }
}