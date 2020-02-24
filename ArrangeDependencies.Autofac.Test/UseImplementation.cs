using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Test.Basis.Factories;
using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
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

            Assert.IsInstanceOf<UserService>(userService);
        }
        
        [Test]
        public void ShouldResolveUseImplementationWhenConstructed()
        {
            var arrange = Arrange.Dependencies(dependencies =>
            {
                dependencies.UseImplementation<IUserFactory, AdminUserFactory>(new AdminUserFactory());
            });

            var userService = arrange.Resolve<IUserFactory>();

            Assert.IsInstanceOf<AdminUserFactory>(userService);
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

            Assert.AreEqual(2, providers.Length);
        }

        [Test]
        public void ShouldResolveMultipleImplementationsFromAssembly()
        {
            var arrange = Arrange.Dependencies(dependencies =>
            {
                dependencies.UseImplementations<IUserFactory>(typeof(IUserFactory).Assembly);
            });

            var providers = arrange.Resolve<IUserFactory[]>();

            Assert.AreEqual(2, providers.Length);
        }
    }
}
