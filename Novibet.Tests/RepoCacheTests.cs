using Novibet.API.Data.Repositories;
using Novibet.Library.Models;

namespace Novibet.Tests
{
    public class Tests
    {
        RepoCache repoCache;
        [SetUp]
        public void Setup()
        {
            this.repoCache = new RepoCache();
        }

        [Test]
        [Description("Tests that IPDetails objects inserted into the cache are stored properly and can be retrieved.")]
        public void CacheInsertAndGet()
        {
            var ip = "195.1.1.1";
            var obj = new IPDetails() { City = "Mosta", Continent = "Europe", Country = "Malta", Ip = ip, Latitude = 12, Longitude = 21 };

            repoCache.InsertIPDetailsIntoCache(obj);
            var objRetrieved = repoCache.GetIPDetailsFromCache(ip);
            Assert.IsNotNull(objRetrieved);
            Assert.AreSame(obj, objRetrieved);
        }
    }
}