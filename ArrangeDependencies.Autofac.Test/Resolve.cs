using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class Resolve
    {       
        [Test]
        public void ShouldAutoResolveMissingDependeciesWithMocks()
        {
            var arrange = Arrange.Dependencies<IUserService, UserService>();

            var userService = arrange.Resolve<IUserService>();

            Assert.IsInstanceOf<UserService>(userService);
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
