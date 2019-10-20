using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using System;
using ArrangeDependencies.Core.Interfaces;
using Autofac;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseEntityExtension
    {

        /// <summary>
        /// Define an <typeparamref name="TEntity"/> to be added to <typeparamref name="TContext"/>.
        /// If <typeparamref name="TContext"/> is not yet defined in the scope, it will be added aswell
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <param name="entityBuilder"></param>
        /// <returns></returns>
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
        /// Define an <typeparamref name="TEntity"/> to be added to <typeparamref name="TContext"/>.
        /// If <typeparamref name="TContext"/> is not yet defined in the scope, it will be added aswell.
        /// Produces the <typeparamref name="TEntity"/> as out parameter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="arrangeBuilder"></param>
        /// <param name="entityBuilder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IArrangeBuilder<ContainerBuilder> UseEntity<TEntity, TContext>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<TEntity> entityBuilder, out TEntity result)
            where TEntity : class
            where TContext : DbContext
        {
            var entity = Activator.CreateInstance<TEntity>();
            entityBuilder(entity);

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
