using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shopping.Database;

namespace Shopping.UnitTest.Infrastructure
{
    public class DatabaseContextFactory : EfContextFactory<DatabaseContext>
    {
        protected override DatabaseContext CreateContext(DbContextOptions<DatabaseContext> options)
        {
            return new DatabaseContext(options);
        }
    }
    
    public abstract class EfContextFactory<TContext> where TContext : DbContext
    {
        public TContext Create(string databaseName)
        {
            DbContextOptionsBuilder<TContext> contextOptionsBuilder = new DbContextOptionsBuilder<TContext>();
            contextOptionsBuilder.UseInMemoryDatabase(databaseName);
            contextOptionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            return CreateContext(contextOptionsBuilder.Options);
        }

        protected abstract TContext CreateContext(DbContextOptions<TContext> options);
    }
}