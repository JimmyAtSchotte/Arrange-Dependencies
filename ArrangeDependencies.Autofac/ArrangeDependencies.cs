using ArrangeDependencies.Core.Interfaces;
using ArrangeDependencies.Autofac.Extensions;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Moq;
using ArrangeDependencies.Autofac.Helpers;

namespace ArrangeDependencies.Autofac
{
    public class ArrangeDependencies : IArrangeDependencies
    {
        private IContainer _container;

        private ArrangeDependencies(IContainer container)
        {
            _container = container;
        }


        public TInterface Resolve<TInterface>()
        {
            return _container.Resolve<TInterface>();
        }

        public static IArrangeDependencies Config(Action<IArrangeBuilder<ContainerBuilder>> config = null)
        {
            var arrangeBuilder = new ArrangeBuilder();
            config?.Invoke(arrangeBuilder);

            var containerBuilder = arrangeBuilder.Build(); 
            var container = containerBuilder.Build();

            return new ArrangeDependencies(container);
        }


        /// <summary>
        /// Looosly configure T dependencies, if you don't define the dependency it will default to a mock of the dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IArrangeDependencies Config<TInterface, TImplementation>(Action<IArrangeBuilder<ContainerBuilder>> config = null) 
            where TInterface : class
            where TImplementation : class
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

        
       
    }
}
