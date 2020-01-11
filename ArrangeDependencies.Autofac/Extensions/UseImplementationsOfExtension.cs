using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Reflection;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseImplementationsOfExtension
    {
        /// <summary>
        /// Using all implementations of T in the assembly of T
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <returns></returns>
        public static IArrangeBuilder<ContainerBuilder> UseImplementationsOf<TInterface>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder)
            where TInterface : class
        {
            if(arrangeBuilder is ArrangeBuilder builder)
                AddImplementationsOf<TInterface>(builder, typeof(TInterface).Assembly);          

            return arrangeBuilder;
        }

        /// <summary>
        /// Unsing all implementations of T in provided assemblies
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <returns></returns>
        public static IArrangeBuilder<ContainerBuilder> UseImplementationsOf<TInterface>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, params Assembly[] assemblies)
            where TInterface : class
        {
            if (arrangeBuilder is ArrangeBuilder builder)            
                AddImplementationsOf<TInterface>(builder, assemblies);            

            return arrangeBuilder;
        }

        private static void AddImplementationsOf<TInterface>(ArrangeBuilder arrangeBuilder, params Assembly[] assemblies)
        {

            
        }
    }
}
