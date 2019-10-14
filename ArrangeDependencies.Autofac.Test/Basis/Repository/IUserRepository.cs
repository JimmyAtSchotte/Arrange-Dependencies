using ArrangeDependencies.Autofac.Test.Basis.Entites;
using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrangeDependencies.Autofac.Test.Basis.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TestDbContext db;
        private IMemoryCache _cache;

        public UserRepository(TestDbContext db, IMemoryCache cache)
        {
            this.db = db;
            _cache = cache;
        }
        

        public User GetByName(string name)
        {
            if (!_cache.TryGetValue(name, out User cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = db.User.FirstOrDefault(x => x.Name == name);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                _cache.Set(name, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}
