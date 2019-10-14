using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using System;

namespace ArrangeDependencies.Autofac.Extensions
{
    public static class UseEntityExtension
    {
        public static ArrangeBuilder UseEntity<TEntity, TContext>(this ArrangeBuilder arrangeBuilder, Action<TEntity> entityBuilder)
            where TEntity : class
            where TContext : DbContext
        {
            var entity = Activator.CreateInstance<TEntity>();
            entityBuilder(entity);

            AddEntity<TEntity, TContext>(arrangeBuilder, entity);

            return arrangeBuilder;
        }

        public static ArrangeBuilder UseEntity<TEntity, TContext>(this ArrangeBuilder arrangeBuilder, Action<TEntity> entityBuilder, out TEntity result)
            where TEntity : class
            where TContext : DbContext
        {
            var entity = Activator.CreateInstance<TEntity>();
            entityBuilder(entity);

            result = AddEntity<TEntity, TContext>(arrangeBuilder, entity);

            return arrangeBuilder;
        }

        private static TEntity AddEntity<TEntity, TContext>(ArrangeBuilder arrangeBuilder, TEntity entity)
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
