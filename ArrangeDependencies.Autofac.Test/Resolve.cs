using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
	public class Resolve
	{
		[Test]
		public void ShouldAutoResolveMissingDependenciesWithMocks()
		{
			var arrange = Arrange.Dependencies<IUserService, UserService>();

			var userService = arrange.Resolve<IUserService>();

			Assert.That(userService, Is.InstanceOf<UserService>());
		}

		[Test]
		public void ShouldAutoResolveLogging()
		{
			var arrange = Arrange.Dependencies<IUserService, UserService>();

			var userService = arrange.Resolve<IUserService>();
			userService.Log();
		}
	}
}