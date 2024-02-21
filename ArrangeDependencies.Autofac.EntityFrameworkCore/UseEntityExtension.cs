using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Microsoft.EntityFrameworkCore;
using System;

namespace ArrangeDependencies.Autofac.EntityFrameworkCore
{
	public static class UseEntityExtension
	{
		/// <summary>
		/// Define a <typeparamref name="TEntity"/> to be added to <typeparamref name="TContext"/>.
		/// If <typeparamref name="TContext"/> is not yet defined in the scope, it will be added as well
		/// </summary>
		public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<TEntity> entityBuilder)
			where TEntity : class
			where TContext : DbContext
		{
			var entity = Activator.CreateInstance<TEntity>();
			entityBuilder(entity);

			AddEntity<TEntity, TContext>(arrangeBuilder, entity);

			return arrangeBuilder;
		}

		/// <summary>
		/// Define a <typeparamref name="TEntity"/> to be added to <typeparamref name="TContext"/>.
		/// If <typeparamref name="TContext"/> is not yet defined in the scope, it will be added aswell.
		/// Produces the <typeparamref name="TEntity"/> as out parameter
		/// </summary>
		public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<TEntity> entityBuilder, out TEntity result)
			where TEntity : class
			where TContext : DbContext
		{
			var entity = Activator.CreateInstance<TEntity>();
			entityBuilder(entity);

			result = AddEntity<TEntity, TContext>(arrangeBuilder, entity);

			return arrangeBuilder;
		}

		public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Func<TEntity> entityBuilder)
		   where TEntity : class
		   where TContext : DbContext
		{
			var entity = entityBuilder();

			AddEntity<TEntity, TContext>(arrangeBuilder, entity);

			return arrangeBuilder;
		}

		public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Func<TEntity> entityBuilder, out TEntity result)
		   where TEntity : class
		   where TContext : DbContext
		{
			var entity = entityBuilder();

			result = AddEntity<TEntity, TContext>(arrangeBuilder, entity);

			return arrangeBuilder;
		}

		public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, TEntity entity)
		where TEntity : class
		where TContext : DbContext
		{
			AddEntity<TEntity, TContext>(arrangeBuilder, entity);

			return arrangeBuilder;
		}

		public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, TEntity entity, out TEntity result)
		   where TEntity : class
		   where TContext : DbContext
		{
			result = AddEntity<TEntity, TContext>(arrangeBuilder, entity);

			return arrangeBuilder;
		}

		private static TEntity AddEntity<TEntity, TContext>(IArrangeBuilder<ContainerBuilder> arrangeBuilder, TEntity entity)
			where TEntity : class
			where TContext : DbContext
		{
			arrangeBuilder.UseDbContext<TContext>(out var db);

			db.Set<TEntity>().Add(entity);
			db.SaveChanges();

			return entity;
		}
	}
}