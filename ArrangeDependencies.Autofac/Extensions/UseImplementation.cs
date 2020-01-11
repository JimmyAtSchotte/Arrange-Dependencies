using ArrangeDependencies.Autofac.Helpers;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseImplementationExtension
    {
        /// <summary>
        /// Define an implementation for a interface. 
        /// If the implementation has undefined dependencies they will default to Mock.Of&lt;T&gt;
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <returns></returns>
        public static IArrangeBuilder<ContainerBuilder> UseImplementation<TInterface, TImplementation>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder)
            where TInterface : class
            where TImplementation : class, TInterface
        {

            if (arrangeBuilder is ArrangeBuilder builder)
                AddDependency<TInterface>(builder, typeof(TImplementation));

            return arrangeBuilder;
        }

        public static IArrangeBuilder<ContainerBuilder> UseImplementations<TInterface>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, params Assembly[] assemblies)
            where TInterface : class
        {
            if (arrangeBuilder is ArrangeBuilder builder)
            {
                var type = typeof(TInterface);
                var implementations = assemblies
                                .SelectMany(s => s.GetTypes())
                                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var implementation in implementations)
                    AddDependency<TInterface>(builder, implementation);
            }
                       

            return arrangeBuilder;
        }

        private static void AddDependency<TInterface>(ArrangeBuilder arrangeBuilder, Type implementation)
            where TInterface : class
        {
            arrangeBuilder.AddDependency((containerBuilder) => containerBuilder.Register((context) =>
            {
                var constructors = implementation.GetConstructors();
                var parameters = constructors.SelectMany(x => x.GetParameters()).Distinct();
                var args = new List<object>();

                foreach (var parameter in parameters)
                    args.Add(context.ResolveOptional(parameter.ParameterType) ?? MockHelper.CreateMock(parameter.ParameterType));

                return Activator.CreateInstance(implementation, args.ToArray()) as TInterface;
            }).As<TInterface>());
        }

       
    }
}
