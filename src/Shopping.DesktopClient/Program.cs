using System;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Proxy;

namespace Shopping.DesktopClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.InstallShoppingProxy();
            services.AddTransient<DesktopClient>();
            var provider = services.BuildServiceProvider();
            var desktopClient = provider.GetService<DesktopClient>();
            desktopClient.RunTest().GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}