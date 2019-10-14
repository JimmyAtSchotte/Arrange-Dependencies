using Autofac;
using Moq;
using System;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseMockExtension
    {
        public static ArrangeBuilder UseMock<T>(this ArrangeBuilder arrangeBuilder, Action<Mock<T>> mock) where T : class
        {
            AddMock(arrangeBuilder, mock);

            return arrangeBuilder;
        }

        public static ArrangeBuilder UseMock<T>(this ArrangeBuilder arrangeBuilder, Action<Mock<T>> mock, out Mock<T> result) where T : class
        { 
            result = AddMock(arrangeBuilder, mock);

            return arrangeBuilder;
        }

        private static Mock<T> AddMock<T>(ArrangeBuilder arrangeBuilder, Action<Mock<T>> mock) where T : class
        {
            var mockObject = new Mock<T>();
            mock.Invoke(mockObject);

            arrangeBuilder.Extend((c) => c.Register<T>((context) => mockObject.Object));

            return mockObject;

        }

    }
}
