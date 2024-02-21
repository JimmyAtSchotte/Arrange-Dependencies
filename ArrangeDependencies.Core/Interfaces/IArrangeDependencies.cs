namespace ArrangeDependencies.Core.Interfaces
{
	public interface IArrangeDependencies
	{
		TInterface Resolve<TInterface>();
	}
}