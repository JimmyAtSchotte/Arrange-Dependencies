using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using ArrangeDependencies.Autofac.Extensions;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class Resolve
    {
        [Test]
        public void ResolveShouldResolveDefinedImplentation()
        {
            var arrange = ArrangeDependencies<UserService>.Config(dependencies => 
            {
                dependencies.UseImplementation((c) => c.RegisterType<UserService>().As<IUserService>());
            });

            var userService = arrange.Resolve();

            Assert.IsInstanceOf<UserService>(userService);
        }

        [Test]
        public void ResolveShouldAutResolveMissingDependeciesWithMocks()
        {
            var arrange = ArrangeDependencies<UserService>.Config();

            var userService = arrange.Resolve();

            Assert.IsInstanceOf<UserService>(userService);
        }
    }
}
