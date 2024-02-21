using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using Moq;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
	public class UseMock
	{
		[Test]
		public void ShouldResolveUsingMock()
		{
			var arrange = Arrange.Dependencies<IUserService, UserService>(dependencies =>
			{
				dependencies.UseMock<IUserRepository>(mock =>
				{
					mock.Setup(x => x.GetByName(It.IsAny<string>())).Returns(new Basis.Entites.User());
				});
			});

			var userService = arrange.Resolve<IUserService>();

			Assert.That(userService, Is.InstanceOf<UserService>());
		}
	}
}