using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Test.Basis;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseDbContext
    {
        [Test]
        public void ShouldUseDbContext()
        {
            var arrange = ArrangeDependencies<UserService>.Config(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
            });

            var testContext = arrange.ResolveDependency<TestDbContext>();

            Assert.IsInstanceOf<TestDbContext>(testContext);
        }

        [Test]
        public void ShouldNotRegisterDbContextTwice()
        {
            var arrange = ArrangeDependencies<UserService>.Config(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
                dependencies.UseDbContext<TestDbContext>();
            });

            var testContexts = arrange.ResolveDependency<TestDbContext[]>();

            Assert.AreEqual(1, testContexts.Length);
        }

    }
}
