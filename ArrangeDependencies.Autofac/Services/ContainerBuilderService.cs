using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Services
{
    public class ContainerBuilderService
    {
        public ContainerBuilder Build(Action<ArrangeBuilder> arrangeBuilderConfig)
        {
            var arrangeBuilder = new ArrangeBuilder();
            arrangeBuilderConfig?.Invoke(arrangeBuilder);
            return arrangeBuilder.Build();
        }
    }
}
