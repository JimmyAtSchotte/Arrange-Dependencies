using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseDbContextExtension
    {
        public static ArrangeBuilder UseDbContext<TContext>(this ArrangeBuilder arrangeBuilder) where TContext : DbContext
        {
            AddDbContext<TContext>(arrangeBuilder);

            return arrangeBuilder;
        }

        public static ArrangeBuilder UseDbContext<TContext>(this ArrangeBuilder arrangeBuilder, out TContext result) where TContext : DbContext
        {
            result = AddDbContext<TContext>(arrangeBuilder);

            return arrangeBuilder;
        }

        private static TContext AddDbContext<TContext>(ArrangeBuilder arrangeBuilder) where TContext : DbContext
        {
            if (arrangeBuilder.IsTypeCached<TContext>(out var result))
                return result;

            var root = new InMemoryDatabaseRoot();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<TContext>(config => config.UseInMemoryDatabase(arrangeBuilder.GetHashCode().ToString(), root));
         
            arrangeBuilder.UseImplementation((c) => c.Populate(serviceCollection));

            var db = serviceCollection.BuildServiceProvider().GetService<TContext>();
            arrangeBuilder.AddTypeToCache(db);

            return db; 
        }

    }
}
