using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using Autofac;
using ArrangeDependencies.Autofac.Extensions;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseImplementation
    {
        [Test]
        public void ShouldResolveUseImplementation()
        {
            var arrange = ArrangeDependencies<UserService>.Config(dependencies =>
            {
                dependencies.UseImplementation((c) => c.RegisterType<UserService>().As<IUserService>());
            });

            var userService = arrange.Resolve();

            Assert.IsInstanceOf<UserService>(userService);
        }
    }
}
