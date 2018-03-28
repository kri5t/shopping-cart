using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Database;

namespace Shopping.Core.Infrastructure
{
    public static class CoreInstaller
    {
        public static IServiceCollection InstallCore(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CoreInstaller).GetTypeInfo().Assembly);
            services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=shopping.db"));
            return services;
        }
    }
}