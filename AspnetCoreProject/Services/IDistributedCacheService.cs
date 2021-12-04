using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Services
{
   public interface IDistributedCacheService
    {
        void SetCache<T>(String key, T values);
        T GetCache<T>(string key) where T : class;
        void RemoveCache(string key);
    }
}
