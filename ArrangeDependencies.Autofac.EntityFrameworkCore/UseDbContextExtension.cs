﻿using ArrangeDependencies.Autofac.Extensions;
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

            var root = new InMemoryDatabaseRoot();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<TContext>(config => config.UseInMemoryDatabase(arrangeBuilder.GetHashCode().ToString(), root)
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

            arrangeBuilder.UseContainerBuilder((c) => c.Populate(serviceCollection));

            var db = serviceCollection.BuildServiceProvider().GetService<TContext>();
            arrangeBuilder.AddTypeToCache(db);

            return db;
        }

    }
}
