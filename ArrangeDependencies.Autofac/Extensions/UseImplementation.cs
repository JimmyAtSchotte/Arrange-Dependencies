using ArrangeDependencies.Autofac.Helpers;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseImplementationExtension
    {
        /// <summary>
        /// Define an implementation for a interface. If the implementation has undefined dependencies they will default to Mock.Of&lt;T&gt;
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <returns></returns>
        public static IArrangeBuilder<ContainerBuilder> UseImplementation<TInterface, TImplementation>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder)
            where TInterface : class
            where TImplementation : class        
        {
            AddDependency(arrangeBuilder as ArrangeBuilder, (containerBuilder) => containerBuilder.Register((context) =>
            {
                var constructors = typeof(TImplementation).GetConstructors();
                var parameters = constructors.SelectMany(x => x.GetParameters()).Where(x => x.ParameterType.IsInterface).Distinct();
                var args = new List<object>();

                foreach (var parameter in parameters)
                    args.Add(context.ResolveOptional(parameter.ParameterType) ?? MockHelper.CreateMock(parameter.ParameterType));

                return Activator.CreateInstance(typeof(TImplementation), args.ToArray()) as TImplementation;
            }).As<TInterface>().IfNotRegistered(typeof(TInterface)));

            return arrangeBuilder;
        }

        private static void AddDependency(ArrangeBuilder arrangeBuilder, Action<ContainerBuilder> dependency)
        {
            arrangeBuilder.AddDependency(dependency);
        }



    }
}
