using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shopping.Database
{
    [UsedImplicitly]
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>(); 
            var configuration = new ConfigurationBuilder()  
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)       
                .AddJsonFile("appsettings.json")
                .Build();

            builder.UseSqlite(configuration.GetConnectionString("DefaultConnection")); 
            return new DatabaseContext(builder.Options); 
        }
    }
}