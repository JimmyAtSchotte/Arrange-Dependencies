using ArrangeDependencies.Autofac.Test.Basis.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Test.Basis
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Company> Company { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> contextOptions) : base(contextOptions)
        {

        }

    }
}
