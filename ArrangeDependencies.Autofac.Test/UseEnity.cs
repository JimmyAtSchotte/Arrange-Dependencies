using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using Autofac;
using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Autofac.Test.Basis.Entites;
using ArrangeDependencies.Autofac.Test.Basis;
using System.Linq;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseEnity
    {
        [Test]
        public void ShouldCreateEntity()
        {
            var arrange = ArrangeDependencies.Config(dependencies =>
            {
                dependencies.UseEntity<User, TestDbContext>((user) => user.SetName("Test name"));
            });

            var db = arrange.Resolve<TestDbContext>();
            var user = db.User.FirstOrDefault();

            Assert.IsNotNull(user);
        }

        [Test]
        public void ShouldCreateRelatedEntity()
        {
            Company company = null;

            var arrange = ArrangeDependencies.Config(dependencies =>
            {
                dependencies.UseEntity<Company, TestDbContext>((company) => company.SetName("Test name"), out company);
                dependencies.UseEntity<User, TestDbContext>((user) => {
                    user.SetName("Test name");
                    user.SetCompany(company);
                });
            });

            var db = arrange.Resolve<TestDbContext>();
            var user = db.User.FirstOrDefault();

            Assert.AreEqual(company.Id, user.CompanyId);
            Assert.AreNotEqual(0, company.Id);
        }

        [Test]
        public void ShouldNotCreateMultipleEntitesOnMultipleResolves()
        {
            Company company = null;

            var arrange = ArrangeDependencies.Config<IUserService, UserService>(dependencies =>
            {
                dependencies.UseEntity<User, TestDbContext>((user) => user.SetName("Test name"));                
            });

            var service = arrange.Resolve<IUserService>();
            var db = arrange.Resolve<TestDbContext>();
            var user = db.User.Count() ;

            Assert.AreEqual(1, db.User.Count());
        }
    }
}
