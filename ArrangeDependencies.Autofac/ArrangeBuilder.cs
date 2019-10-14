using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;

namespace ArrangeDependencies.Autofac
{
    public class ArrangeBuilder : IArrangeBuilder
    {
        private readonly List<Action<ContainerBuilder>> _dependencies;
        private readonly Dictionary<Type, object> _cachedTypes;


        public ArrangeBuilder()
        {
            _dependencies = new List<Action<ContainerBuilder>>();
            _cachedTypes = new Dictionary<Type, object>();
        }

        public void AddTypeToCache<T>(T value) where T : class => AddTypeToCache(typeof(T), value) ;
        

        public void AddTypeToCache(Type type, object value)
        {
            _cachedTypes.Add(type, value);
        }

        public bool IsTypeCached<T>(out T result) where T : class
        {
            var isRegisterd = _cachedTypes.TryGetValue(typeof(T), out var value);
            result = value as T;

            return isRegisterd;
        }       

        internal ArrangeBuilder Extend(Action<ContainerBuilder> dependency)
        {
            _dependencies.Add(dependency);

            return this;
        }

        internal ContainerBuilder Build()
        {
            var containerBuilder = new ContainerBuilder();
            _dependencies.ForEach(x => x?.Invoke(containerBuilder));
            return containerBuilder;
        }
    }
}
