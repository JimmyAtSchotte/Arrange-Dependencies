using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Test.Basis;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using Autofac;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseDbContext
    {
        [Test]
        public void ShouldUseDbContext()
        {
            var arrange = ArrangeDependencies.Config(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
            });

            var testContext = arrange.Resolve<TestDbContext>();

            Assert.IsInstanceOf<TestDbContext>(testContext);
        }

        [Test]
        public void ShouldNotRegisterDbContextTwice()
        {
            var arrange = ArrangeDependencies.Config(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
                dependencies.UseDbContext<TestDbContext>();
            });

            var testContexts = arrange.Resolve<TestDbContext[]>();

            Assert.AreEqual(1, testContexts.Length);
        }

    }
}
