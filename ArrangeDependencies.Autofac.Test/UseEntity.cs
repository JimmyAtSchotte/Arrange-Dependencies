using ArrangeDependencies.Autofac.EntityFrameworkCore;
using ArrangeDependencies.Autofac.Test.Basis;
using ArrangeDependencies.Autofac.Test.Basis.Entites;
using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using System.Linq;

namespace ArrangeDependencies.Autofac.Test
{
	public class UseEntity
	{
		[Test]
		public void ShouldCreateEntity()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseEntity<User, TestDbContext>((user) => user.SetName("Test name"));
			});

			var db = arrange.Resolve<TestDbContext>();
			var user = db.User.FirstOrDefault();

			Assert.That(user, Is.Not.Null);
		}

		[Test]
		public void ShouldCreateRelatedEntity()
		{
			Company company = null;

			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseEntity<Company, TestDbContext>((company) => company.SetName("Test name"), out company);
				dependencies.UseEntity<User, TestDbContext>((user) =>
				{
					user.SetName("Test name");
					user.SetCompany(company);
				});
			});

			var db = arrange.Resolve<TestDbContext>();
			var user = db.User.FirstOrDefault();

			Assert.That(user.CompanyId, Is.EqualTo(company.Id));
			Assert.That(company.Id, Is.Not.EqualTo(0));
		}

		[Test]
		public void ShouldCreateRelatedEntity2()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseEntity<Company, TestDbContext>((company) => company.SetName("Test name"), out var company);
				dependencies.UseEntity<User, TestDbContext>((user) =>
				{
					user.SetName("Test name");
					user.SetCompany(company);
				});
			});

			var db = arrange.Resolve<TestDbContext>();
			var user = db.User.FirstOrDefault();

			Assert.That(user.CompanyId, Is.EqualTo(1));
		}

		[Test]
		public void ShouldAddEntityFromFunc()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseEntity<Company, TestDbContext>(() => new Company());
			});

			var db = arrange.Resolve<TestDbContext>();
			var company = db.Company.FirstOrDefault();

			Assert.That(company.Id, Is.EqualTo(1));
		}

		[Test]
		public void ShouldAddEntity()
		{
			var arrange = Arrange.Dependencies(dependencies =>
			{
				dependencies.UseEntity<Company, TestDbContext>(new Company());
			});

			var db = arrange.Resolve<TestDbContext>();
			var company = db.Company.FirstOrDefault();

			Assert.That(company.Id, Is.EqualTo(1));
		}

		[Test]
		public void ShouldNotCreateMultipleEntitiesOnMultipleResolves()
		{
			var arrange = Arrange.Dependencies<IUserService, UserService>(dependencies =>
			{
				dependencies.UseEntity<User, TestDbContext>((user) => user.SetName("Test name"));
			});

			var service = arrange.Resolve<IUserService>();
			var db = arrange.Resolve<TestDbContext>();

			Assert.That(db.User.Count(), Is.EqualTo(1));
		}

		[Test]
		public void ShouldCreateMultipleInstances()
		{
			for (int i = 0; i < 25; i++)
			{
				var arrange = Arrange.Dependencies<IUserService, UserService>(dependencies =>
				{
					dependencies.UseEntity<User, TestDbContext>((user) => user.SetName("Test name"));
				});

				var db = arrange.Resolve<TestDbContext>();

				Assert.That(db.User.Count(), Is.EqualTo(1));
			}
		}
	}
}