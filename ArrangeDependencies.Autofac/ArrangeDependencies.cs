using ArrangeDependencies.Core.Interfaces;
using ArrangeDependencies.Autofac.Extensions;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Moq;

namespace ArrangeDependencies.Autofac
{
    public class ArrangeDependencies<T> : IArrange<T> where T : class
    {
        private IContainer _container;

        private ArrangeDependencies(IContainer container)
        {
            _container = container;
        }

        public T Resolve()
        {
            return _container.Resolve<T>();
        }

        public TInterface ResolveDependency<TInterface>()
        {
            return _container.Resolve<TInterface>();
        }

        public static IArrange<T> Config(Action<ArrangeBuilder> config = null)
        {
            var arrangeBuilder = new ArrangeBuilder();
            config?.Invoke(arrangeBuilder);
                       
            var containerBuilder = arrangeBuilder.Build();
            containerBuilder.Register((context) =>
            {
                var constructors = typeof(T).GetConstructors();
                var parameters = constructors.SelectMany(x => x.GetParameters()).Where(x => x.ParameterType.IsInterface).Distinct();
                var args = new List<object>();

                foreach (var parameter in parameters)
                    args.Add(context.ResolveOptional(parameter.ParameterType) ?? CreateMock(parameter.ParameterType));

                return Activator.CreateInstance(typeof(T), args.ToArray()) as T;
            }).As<T>().IfNotRegistered(typeof(T));

            var container = containerBuilder.Build();
                       
            return new ArrangeDependencies<T>(container);
        }

        private static object CreateMock(Type parameterType)
        {
            var mockType = typeof(Mock<>).MakeGenericType(parameterType);
            var mock = Activator.CreateInstance(mockType);
            return ((Mock)mock).Object;
        }

       
    }
}
