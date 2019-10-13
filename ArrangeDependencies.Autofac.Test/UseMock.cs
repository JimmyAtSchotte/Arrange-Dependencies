using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using ArrangeDependencies.Autofac.Extensions;
using Moq;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseMock
    {
        [Test]
        public void ShouldResolveUsingMock()
        {
            var arrange = ArrangeDependencies<UserService>.Config(dependencies =>
            {
                dependencies.UseMock<IUserRepository>(mock => Mock.Of<IUserRepository>());
            });

            var userService = arrange.Resolve();

            Assert.IsInstanceOf<UserService>(userService);
        }

    }
}
