using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseContainerBuilderExtension
    {
        public static IArrangeBuilder<ContainerBuilder> UseContainerBuilder(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<ContainerBuilder> dependency)
        {
            AddDependency(arrangeBuilder as ArrangeBuilder, dependency);

            return arrangeBuilder;
        }

        private static void AddDependency(ArrangeBuilder arrangeBuilder, Action<ContainerBuilder> dependency)
        {
            arrangeBuilder.AddDependency(dependency);
        }

    }
}
