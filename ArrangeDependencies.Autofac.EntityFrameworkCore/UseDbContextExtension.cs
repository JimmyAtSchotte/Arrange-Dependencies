using System;
using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ArrangeDependencies.Autofac.EntityFrameworkCore
{
    public static class UseDbContextExtension
    {

        /// <summary>
        /// Define an <typeparamref name="TContext"/> to be used in the arrange scope 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <returns></returns>
        public static IArrangeBuilder<ContainerBuilder> UseDbContext<TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder)
            where TContext : DbContext
        {
            AddDbContext<TContext>(arrangeBuilder as ArrangeBuilder);

            return arrangeBuilder;
        }

        public static IArrangeBuilder<ContainerBuilder> UseDbContext<TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, out TContext result)
            where TContext : DbContext
        {
            result = AddDbContext<TContext>(arrangeBuilder as ArrangeBuilder);

            return arrangeBuilder;
        }

        private static TContext AddDbContext<TContext>(ArrangeBuilder arrangeBuilder)
            where TContext : DbContext
        {
            if (arrangeBuilder.IsTypeCached<TContext>(out var result))
                return result;
            
            var dbName = Guid.NewGuid().ToString();
            var rootProvider = new ServiceCollection()
                .AddDbContext<TContext>(o => o.UseInMemoryDatabase(dbName))
                .BuildServiceProvider();

            var db = rootProvider.GetService<TContext>();
            
            arrangeBuilder.UseContainerBuilder((c) => c.Register(s => db));
            arrangeBuilder.AddTypeToCache(db);

            return db;
        }

    }
}
