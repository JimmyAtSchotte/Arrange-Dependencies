using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ArrangeDependencies.Autofac.MemoryCache
{
    public static class UseMemoryCacheExtension
    {
        public static IArrangeBuilder<ContainerBuilder> UseMemoryCache(this IArrangeBuilder<ContainerBuilder> arrangeBuilder)
        {
            AddMemoryCache(arrangeBuilder as ArrangeBuilder);

            return arrangeBuilder;
        }

        public static IArrangeBuilder<ContainerBuilder> UseMemoryCache<T>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, object key, T value)
        {
            var memoryCache = AddMemoryCache(arrangeBuilder as ArrangeBuilder);
            memoryCache.Set(key, value);

            return arrangeBuilder;
        }
        
        private static IMemoryCache AddMemoryCache(ArrangeBuilder arrangeBuilder)
        {
            if (arrangeBuilder.IsTypeCached<IMemoryCache>(out var result))
                return result;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryCache();
            var cache = serviceCollection.BuildServiceProvider().GetService<IMemoryCache>();          

            arrangeBuilder.UseContainerBuilder((c) => c.Register(t => cache).As<IMemoryCache>());
            arrangeBuilder.AddTypeToCache(cache);

            return cache;
        }
    }
}
