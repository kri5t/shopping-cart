using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shopping.UnitTest.Infrastructure
{
    public abstract class EfContextTest<TContextFactory, TContext> : IDisposable where TContextFactory : EfContextFactory<TContext>, new() where TContext : DbContext
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private readonly string _contextDatabaseName;

        protected EfContextTest()
        {
            _contextDatabaseName = Guid.NewGuid().ToString();
        }

        protected TContext Context()
        {
            var context = Activator.CreateInstance<TContextFactory>().Create(_contextDatabaseName);
            _disposables.Add(context);
            return context;
        }

        public void Dispose()
        {
            Disposing();
            _disposables.ForEach(x => x.Dispose());
        }

        protected void RegisterForDisposal(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        protected virtual void Disposing()
        {
        }
    }   
}