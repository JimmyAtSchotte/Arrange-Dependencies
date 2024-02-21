using ArrangeDependencies.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace ArrangeDependencies.Core.SharedKernel
{
	public abstract class BaseArrangeBuilder<T> : IArrangeBuilder<T>
		where T : class
	{
		private readonly List<Action<T>> _dependencies = new();
		private readonly Dictionary<Type, object> _cachedTypes = new();

		public virtual void AddTypeToCache<TObject>(TObject value) where TObject : class => AddTypeToCache(typeof(TObject), value);

		protected virtual void AddTypeToCache(Type type, object value)
		{
			_cachedTypes.Add(type, value);
		}

		public virtual bool IsTypeCached<TObject>(out TObject result) where TObject : class
		{
			var isRegistered = _cachedTypes.TryGetValue(typeof(TObject), out var value);

			result = value as TObject;

			return isRegistered;
		}

		public virtual BaseArrangeBuilder<T> AddDependency(Action<T> dependency)
		{
			_dependencies.Add(dependency);

			return this;
		}

		public virtual T Build()
		{
			var containerBuilder = Activator.CreateInstance(typeof(T)) as T;

			foreach (var dependency in _dependencies)
				dependency.Invoke(containerBuilder);

			return containerBuilder;
		}
	}
}