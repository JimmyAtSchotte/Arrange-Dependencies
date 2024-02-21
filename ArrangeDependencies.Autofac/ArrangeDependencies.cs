using ArrangeDependencies.Core.Interfaces;
using Autofac;

namespace ArrangeDependencies.Autofac
{
	public class ArrangeDependencies : IArrangeDependencies
	{
		private readonly IContainer _container;

		internal ArrangeDependencies(IContainer container)
		{
			_container = container;
		}

		public TInterface Resolve<TInterface>()
		{
			return _container.Resolve<TInterface>();
		}
	}
}