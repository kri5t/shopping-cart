using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Database;

namespace Shopping.Core.Infrastructure
{
    public static class CoreInstaller
    {
        public static IServiceCollection InstallCore(this IServiceCollection services, string databaseConnectionString)
        {
            var currentAssembly = typeof(CoreInstaller).GetTypeInfo().Assembly;
            services.AddMediatR(currentAssembly);
            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(databaseConnectionString));
            return services;
        }
    }
}