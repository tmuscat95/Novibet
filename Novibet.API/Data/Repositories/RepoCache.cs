using Novibet.Library.Models;
using System.Runtime.Caching;

namespace Novibet.API.Data.Repositories
{
    public class RepoCache : IRepoCache
    {
        private CacheItemPolicy policy;
        private ObjectCache cache = MemoryCache.Default;
        
        public RepoCache()
        {
            policy = new CacheItemPolicy();
        }
        public void InsertIPDetailsIntoCache(IPDetails iPDetails)
        {
            cache.Set(iPDetails.Ip, iPDetails, policy);
            
        }

        public IPDetails? GetIPDetailsFromCache(string ip)
        {
            return (IPDetails?)cache.Get(ip);
        }

        public void ClearCache()
        {
            List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }

    }
}
