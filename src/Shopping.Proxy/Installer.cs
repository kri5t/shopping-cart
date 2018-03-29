using Microsoft.Extensions.DependencyInjection;

namespace Shopping.Proxy
{
    public static class Installer
    {
        public static IServiceCollection InstallShoppingProxy(this IServiceCollection services)
        {
            return services;
        } 
    }
}