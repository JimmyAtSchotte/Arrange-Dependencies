using ArrangeDependencies.Autofac.EntityFrameworkCore;
using ArrangeDependencies.Autofac.Test.Basis;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
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

			Assert.That(testContext, Is.InstanceOf<TestDbContext>());
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

			Assert.That(testContexts.Length, Is.EqualTo(1));
		}
	}
}