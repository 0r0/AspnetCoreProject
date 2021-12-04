using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspnetCoreProject.Services
{
    public class DistributedCacheService : IDistributedCacheService
    {
        public IDistributedCache _cache;
        public DistributedCacheService(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public T GetCache<T>(string key) where T : class
        {
            try {
                byte[] values = _cache.Get(key);
                if (values == null)
                    return null;
                T result = JsonSerializer.Deserialize<T>(values);
                return result;
            }
            catch(Exception){ }
                return null;
        }
        public void SetCache<T>(string key, T values)
        {
            try
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(values, typeof(T));
                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions() {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                    SlidingExpiration=TimeSpan.FromSeconds(30)
                };
                _cache.Set(key, bytes, cacheOptions);
            }
            catch (Exception)
            {

            }
        }

        public void RemoveCache(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch (Exception) { }
        }

       
    }
}
