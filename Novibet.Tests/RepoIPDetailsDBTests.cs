using Microsoft.EntityFrameworkCore;
using Novibet.API.Data;
using Novibet.API.Data.Repositories;
using Novibet.Library.Models;

namespace Novibet.Tests
{


    class RepoIPDetailsDBTests
    {
        RepoIPDetailsDB repoIPDetailsDB;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<IPDetailsDBContext>()
                .UseInMemoryDatabase(databaseName: "IPDetailsDB")
                .Options;
            this.repoIPDetailsDB = new RepoIPDetailsDB(new API.Data.IPDetailsDBContext(options));
        }

        [Test]
        [Description("Tests that repo can store and retrieve a given object in the database.")]
        public async Task IPDetailsInsertAndRetrieve()
        {
            var ip = "195.1.1.1";

            var obj = new IPDetails() { City = "Mosta", Continent = "Europe", Country = "Malta", Ip = ip, Latitude = 12, Longitude = 21 };
            await this.repoIPDetailsDB.InsertIPDetailsIntoDB(obj);
            var retrieved = await this.repoIPDetailsDB.GetIPDetailsFromDB(ip);
            Assert.That(retrieved, Is.SameAs(obj));
        }

        [Test]
        [Description("Tests that repo can find and update a record in the database.")]
        public async Task IPDetailsUpdate()
        {
            var ip = "195.1.1.1";
            var obj = new IPDetails() { City = "Mosta", Continent = "Europe", Country = "Malta", Ip = ip, Latitude = 12, Longitude = 21 };
            await this.repoIPDetailsDB.InsertIPDetailsIntoDB(obj);
            var objUpdated = new IPDetails() { City = "Naxxar", Continent = "Europe", Country = "Malta", Ip = ip, Latitude = 13, Longitude = 22 };
            await this.repoIPDetailsDB.UpdateIPDetailsInDB(objUpdated);

            var retrieved = await this.repoIPDetailsDB.GetIPDetailsFromDB(ip);
            Assert.True(retrieved?.Equals(objUpdated) ?? false);
        }

    }
}
