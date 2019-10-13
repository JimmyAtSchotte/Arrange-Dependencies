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
            var arrange = ArrangeDependencies<UserService>.Config();

            var userService = arrange.Resolve();

            Assert.IsInstanceOf<UserService>(userService);
        }
    }
}
