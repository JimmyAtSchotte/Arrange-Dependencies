using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Repository;
using ArrangeDependencies.Autofac.InMemoryCache;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Memory;
using ArrangeDependencies.Autofac.Test.Basis.Entites;
using ArrangeDependencies.Autofac.EntityFrameworkCore;
using ArrangeDependencies.Autofac.Test.Basis;

namespace ArrangeDependencies.Autofac.Test
{
    public class UseInMemoryCache
    {
        [Test]
        public void ShouldResolveClassWithInMemoryCacheDependency()
        {
            var arrange = ArrangeDependencies.Config<IUserRepository, UserRepository>(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
            });

            var userRepository = arrange.Resolve<IUserRepository>();

            Assert.IsInstanceOf<UserRepository>(userRepository);
        }

        [Test]
        public void ShouldReturnValueInCache()
        {
            var user = new User();
            user.SetName("Test");

            var arrange = ArrangeDependencies.Config<IUserRepository, UserRepository>(dependencies =>
            {
                dependencies.UseDbContext<TestDbContext>();
                dependencies.UseInMemoryCache("Test", user);
            });

            var userRepository = arrange.Resolve<IUserRepository>();
            var result = userRepository.GetByName("Test");

            Assert.AreEqual(user.Name, result?.Name);

        }

    }
}
