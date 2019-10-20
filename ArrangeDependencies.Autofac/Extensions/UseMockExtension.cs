using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Moq;
using System;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseMockExtension
    {
        public static IArrangeBuilder<ContainerBuilder> UseMock<TMock>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<Mock<TMock>> mock) 
            where TMock : class
        {
            AddMock(arrangeBuilder as ArrangeBuilder, mock);

            return arrangeBuilder;
        }

        public static IArrangeBuilder<ContainerBuilder> UseMock<TMock>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<Mock<TMock>> mock, out Mock<TMock> result) 
            where TMock : class
        { 
            result = AddMock(arrangeBuilder as ArrangeBuilder, mock);

            return arrangeBuilder;
        }

        private static Mock<T> AddMock<T>(ArrangeBuilder arrangeBuilder, Action<Mock<T>> mock) where T : class
        {
            var mockObject = new Mock<T>();
            mock.Invoke(mockObject);

            arrangeBuilder.AddDependency((c) => c.Register<T>((context) => mockObject.Object));

            return mockObject;
        }
    }
}
