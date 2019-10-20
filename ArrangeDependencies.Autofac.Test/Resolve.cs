using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class Resolve
    {       
        [Test]
        public void ShouldAutResolveMissingDependeciesWithMocks()
        {
            var arrange = ArrangeDependencies.Config<IUserService, UserService>();

            var userService = arrange.Resolve<IUserService>();

            Assert.IsInstanceOf<UserService>(userService);
        }
    }
}
