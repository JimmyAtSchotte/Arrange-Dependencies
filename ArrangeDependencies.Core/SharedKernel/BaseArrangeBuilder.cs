using ArrangeDependencies.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Core.SharedKernel
{
    public abstract class BaseArrangeBuilder<T> : IArrangeBuilder<T> where T : class
    {
        protected readonly List<Action<T>> _dependencies;
        protected readonly Dictionary<Type, object> _cachedTypes;


        public BaseArrangeBuilder()
        {
            _dependencies = new List<Action<T>>();
            _cachedTypes = new Dictionary<Type, object>();
        }

        public virtual void AddTypeToCache<TObject>(TObject value) where TObject : class => AddTypeToCache(typeof(TObject), value);

        public virtual void AddTypeToCache(Type type, object value)
        {
            _cachedTypes.Add(type, value);
        }

        public virtual bool IsTypeCached<TObject>(out TObject result) where TObject : class
        {
            var isRegisterd = _cachedTypes.TryGetValue(typeof(TObject), out var value);
            result = value as TObject;

            return isRegisterd;
        }

        public virtual BaseArrangeBuilder<T> AddDependency(Action<T> dependency)
        {
            _dependencies.Add(dependency);

            return this;
        }

        public virtual T Build()
        {
            var containerBuilder =  Activator.CreateInstance(typeof(T)) as T;

            foreach (var dependency in _dependencies)            
                dependency.Invoke(containerBuilder);
            
            return containerBuilder;
        }
    }
}
