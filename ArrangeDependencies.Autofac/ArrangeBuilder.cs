using ArrangeDependencies.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;

namespace ArrangeDependencies.Autofac
{
    public class ArrangeBuilder : IArrangeBuilder
    {
        private readonly List<Action<ContainerBuilder>> _dependencies;
        
        public ArrangeBuilder()
        {
            _dependencies = new List<Action<ContainerBuilder>>();
        }
        
        internal ArrangeBuilder Extend(Action<ContainerBuilder> dependency)
        {
            _dependencies.Add(dependency);

            return this;
        }

        internal ContainerBuilder Build()
        {
            var containerBuilder = new ContainerBuilder();
            _dependencies.ForEach(x => x?.Invoke(containerBuilder));
            return containerBuilder;
        }
    }
}
