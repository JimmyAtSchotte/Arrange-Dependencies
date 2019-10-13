using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseImplementationExtension
    {
        public static ArrangeBuilder UseImplementation(this ArrangeBuilder arrangeBuilder, Action<ContainerBuilder> dependency)
        {
            arrangeBuilder.Extend(dependency);

            return arrangeBuilder;
        }

    }
}
