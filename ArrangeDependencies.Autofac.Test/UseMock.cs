using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using ArrangeDependencies.Autofac.Extensions;
using Moq;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseMock
    {
        [Test]
        public void ShouldResolveUsingMock()
        {
            var arrange = Arrange.Dependencies<IUserService, UserService>(dependencies =>
            {
                dependencies.UseMock<IUserRepository>(mock => {
                    mock.Setup(x => x.GetByName(It.IsAny<string>())).Returns(new Basis.Entites.User());
                });
            });

            var userService = arrange.Resolve<IUserService>();

            Assert.IsInstanceOf<UserService>(userService);
        }    
    }
}
