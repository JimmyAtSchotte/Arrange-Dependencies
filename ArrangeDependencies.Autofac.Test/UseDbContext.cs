using ArrangeDependencies.Autofac.EntityFrameworkCore;
using ArrangeDependencies.Autofac.Test.Basis;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseDbContext
    {
        [Test]
        public void ShouldUseDbContext()
        {
            var arrange = Arrange.Dependencies(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
            });

            var testContext = arrange.Resolve<TestDbContext>();

            Assert.IsInstanceOf<TestDbContext>(testContext);
        }

        [Test]
        public void ShouldNotRegisterDbContextTwice()
        {
            var arrange = Arrange.Dependencies(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
                dependencies.UseDbContext<TestDbContext>();
            });

            var testContexts = arrange.Resolve<TestDbContext[]>();

            Assert.AreEqual(1, testContexts.Length);
        }

    }
}
