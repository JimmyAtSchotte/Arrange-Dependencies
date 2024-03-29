﻿using Moq;
using System;

namespace ArrangeDependencies.Autofac.Helpers
{
	public class MockHelper
	{
		public static object CreateMock(Type parameterType)
		{
			var mockType = typeof(Mock<>).MakeGenericType(parameterType);
			var mock = Activator.CreateInstance(mockType);
			return ((Mock)mock).Object;
		}
	}
}