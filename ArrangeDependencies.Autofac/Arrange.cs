using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Helpers;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArrangeDependencies.Autofac
{
	public class Arrange
	{
		public static IArrangeDependencies Dependencies(Action<IArrangeBuilder<ContainerBuilder>> config = null)
		{
			var arrangeBuilder = new ArrangeBuilder();
			config?.Invoke(arrangeBuilder);

			var containerBuilder = arrangeBuilder.Build();
			var container = containerBuilder.Build();

			return new ArrangeDependencies(container);
		}

		/// <summary>
		/// Loosely configure dependencies of TInterface. If you don't define a dependency it will default to a mock of it.
		/// Use this entry point when you want to test using an interface of a concrete class.
		/// </summary>
		/// <typeparam name="TInterface">The interface you want to test.</typeparam>
		/// <typeparam name="TImplementation">The concrete class you want to test.</typeparam>
		public static IArrangeDependencies Dependencies<TInterface, TImplementation>(Action<IArrangeBuilder<ContainerBuilder>> config = null)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			var arrangeBuilder = new ArrangeBuilder();
			config?.Invoke(arrangeBuilder);

			arrangeBuilder.UseImplementation<TInterface, TImplementation>();

			var containerBuilder = arrangeBuilder.Build();
			containerBuilder.Register((context) =>
			{
				var constructors = typeof(TImplementation).GetConstructors();
				var parameters = constructors.SelectMany(x => x.GetParameters()).Where(x => x.ParameterType.IsInterface).Distinct();
				var args = new List<object>();

				foreach (var parameter in parameters)
					args.Add(context.ResolveOptional(parameter.ParameterType) ?? MockHelper.CreateMock(parameter.ParameterType));

				return Activator.CreateInstance(typeof(TImplementation), args.ToArray()) as TImplementation;
			}).As<TInterface>().IfNotRegistered(typeof(TInterface));

			var container = containerBuilder.Build();

			return new ArrangeDependencies(container);
		}

		/// <summary>
		/// Loosely configure dependencies of TImplementation. If you don't define a dependency it will default to a mock of it.
		/// Use this entry point when you want to test a concrete class directly.
		/// </summary>
		/// <typeparam name="TImplementation">The concrete class you want to test.</typeparam>
		public static IArrangeDependencies Dependencies<TImplementation>(Action<IArrangeBuilder<ContainerBuilder>> config = null)
			where TImplementation : class
		{
			return Dependencies<TImplementation, TImplementation>(config);
		}
	}
}