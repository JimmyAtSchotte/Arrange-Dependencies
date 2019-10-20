using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ArrangeDependencies.Autofac.InMemoryCache
{
    public static class UseInMemoryCacheExtension
    {
        public static IArrangeBuilder<ContainerBuilder> UseInMemoryCache(this IArrangeBuilder<ContainerBuilder> arrangeBuilder)
        {
            AddInMemoryCache(arrangeBuilder as ArrangeBuilder);

            return arrangeBuilder;
        }

        public static IArrangeBuilder<ContainerBuilder> UseInMemoryCache<T>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, object key, T value)
        {
            var memoryCache = AddInMemoryCache(arrangeBuilder as ArrangeBuilder);
            memoryCache.Set(key, value);

            return arrangeBuilder;
        }
        
        private static IMemoryCache AddInMemoryCache(ArrangeBuilder arrangeBuilder)
        {
            if (arrangeBuilder.IsTypeCached<IMemoryCache>(out var result))
                return result;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryCache();
            var cache = serviceCollection.BuildServiceProvider().GetService<IMemoryCache>();          

            arrangeBuilder.UseContainerBuilder((c) => c.Register(c => cache).As<IMemoryCache>());
            arrangeBuilder.AddTypeToCache(cache);

            return cache;
        }
    }
}
