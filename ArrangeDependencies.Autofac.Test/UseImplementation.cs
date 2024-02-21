using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Test.Basis.Factories;
using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
	public class UseImplementation
	{
		[Test]
		public void ShouldResolveUseImplementation()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseImplementation<IUserService, UserService>();
			});

			var userService = arrange.Resolve<IUserService>();

			Assert.That(userService, Is.InstanceOf<UserService>());
		}

		[Test]
		public void ShouldResolveUseImplementationWhenConstructed()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseImplementation<IUserFactory, AdminUserFactory>(new AdminUserFactory());
			});

			var userService = arrange.Resolve<IUserFactory>();

			Assert.That(userService, Is.InstanceOf<AdminUserFactory>());
		}

		[Test]
		public void ShouldResolveMultipleImplementations()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseImplementation<IUserFactory, AdminUserFactory>();
				dependencies.UseImplementation<IUserFactory, BasicUserFactory>();
			});

			var providers = arrange.Resolve<IUserFactory[]>();

			Assert.That(providers.Length, Is.EqualTo(2));
		}

		[Test]
		public void ShouldResolveMultipleImplementationsFromAssembly()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseImplementations<IUserFactory>(typeof(IUserFactory).Assembly);
			});

			var providers = arrange.Resolve<IUserFactory[]>();

			Assert.That(providers.Length, Is.EqualTo(2));
		}
	}
}