using Autofac;
using Moq;
using System;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseMockExtension
    {
        public static ArrangeBuilder UseMock<T>(this ArrangeBuilder arrangeBuilder, Action<Mock<T>> mock) where T : class
        {
            var mockObject = new Mock<T>();
            mock.Invoke(mockObject);

            arrangeBuilder.Extend((c) => c.Register<T>((context) => mockObject.Object));

            return arrangeBuilder;
        }

        public static ArrangeBuilder UseMock<T>(this ArrangeBuilder arrangeBuilder, Action<Mock<T>> mock, out Mock<T> result) where T : class
        {
            var mockObject = new Mock<T>();
            mock.Invoke(mockObject);
            result = mockObject;

            arrangeBuilder.Extend((c) => c.Register<T>((context) => mockObject.Object));

            return arrangeBuilder;
        }

    }
}
