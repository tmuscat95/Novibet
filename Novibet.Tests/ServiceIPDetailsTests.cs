using Microsoft.EntityFrameworkCore;
using Novibet.API.Data;
using Novibet.API.Data.Repositories;
using Novibet.API.Types;
using Novibet.Library;
using Novibet.Library.Models;
using System.Text.RegularExpressions;

namespace Novibet.Tests
{
    public class ServiceIPDetailsTests
    {
        ServiceIPDetails serviceIPDetails;
        RepoIPDetailsDB repoIPDetailsDB;
        RepoCache repoCache;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<IPDetailsDBContext>()
                .UseInMemoryDatabase(databaseName: "IPDetailsDB")
                .Options;
            this.repoCache = new RepoCache();
            this.repoIPDetailsDB = new RepoIPDetailsDB(new API.Data.IPDetailsDBContext(options));
            this.serviceIPDetails = new ServiceIPDetails(repoCache, repoIPDetailsDB, new IPInfoProvider());
            repoIPDetailsDB.TruncateIPDetailsTable();
            repoCache.ClearCache();
        }


        [Test]
        [Description("Tests that Add Batch Job Places A new Job in the static container.")]
        public async Task AddBatchJobTest()
        {
            var updateJob = new UpdateJob(20);
            var guid = serviceIPDetails.AddBatchJob(updateJob);
            var retrievedUpdateJob = ServiceIPDetails.GetJob(guid);
            Assert.IsNotNull(retrievedUpdateJob);
            Assert.That(retrievedUpdateJob, Is.SameAs(updateJob));
        }

        [Test]
        [Description("Tests that GetJobProgress returns a properly formatted string for an existing job guid and an empty string for a non-existent job guid.")]
        public void GetJobProgressTest()
        {
            Assert.IsEmpty(serviceIPDetails.GetJobProgress(Guid.NewGuid()));
            var updateJob = new UpdateJob(20);
            var guid = serviceIPDetails.AddBatchJob(updateJob);
            var jobProgress = serviceIPDetails.GetJobProgress(guid);
            Assert.IsNotEmpty(jobProgress);
            Assert.IsTrue(Regex.IsMatch(jobProgress, "[0-9]+/[0-9]+"));
        }

        [Test]
        [Description("Tests that GetIPDetails returns correct results and stores them in the database and cache")]
        public async Task GetIPDetailsTest()
        {
            var ip = "8.8.8.8"; //Google
            var ipDetails = await serviceIPDetails.GetIPDetails(ip);
            Assert.IsNotNull(ipDetails);
            Assert.IsTrue((ipDetails.Country ?? String.Empty) ==  "United States");
            var ipDetailsFromCache = repoCache.GetIPDetailsFromCache(ip);
            Assert.IsNotNull(ipDetailsFromCache);
            var ipDetailsFromDb = await repoIPDetailsDB.GetIPDetailsFromDB(ip);
            Assert.IsNotNull(ipDetailsFromDb);

            Assert.IsTrue(ipDetailsFromCache.Equals(ipDetails));
            Assert.IsTrue(ipDetailsFromDb.Equals(ipDetails));
        }

        [Test]
        [Description("Tests Updating single IPDetails ")]
        public async Task UpdateTest()
        {
            var ip = "8.8.8.8"; //Google
            var ipDetails = await serviceIPDetails.GetIPDetails(ip);
            Assert.IsNotNull(ipDetails);
            ipDetails.Country = "China";
            await serviceIPDetails.Update(ipDetails);

            var ipDetailsFromCache = repoCache.GetIPDetailsFromCache(ip);
            Assert.IsNotNull(ipDetailsFromCache);
            var ipDetailsFromDb = await repoIPDetailsDB.GetIPDetailsFromDB(ip);
            Assert.IsNotNull(ipDetailsFromDb);

            Assert.IsTrue(ipDetailsFromCache.Ip == ip && ipDetailsFromCache.Country == "China");
            Assert.IsTrue(ipDetailsFromDb.Ip == ip && ipDetailsFromDb.Country == "China");
        }

        [Test]
        [Description("Tests Batch Updating")]
        public async Task BatchUpdateTest()
        {
            var ip1 = "8.8.8.8"; //Google
            var ipDetails1 = await serviceIPDetails.GetIPDetails(ip1);
            var ip2 = "66.220.144.0"; //Facebook
            var ipDetails2 = await serviceIPDetails.GetIPDetails(ip2);
            var ip3 = "176.32.103.205"; //Amazon
            var ipDetails3 = await serviceIPDetails.GetIPDetails(ip3);

            Assert.IsNotNull(ipDetails3);
            Assert.IsNotNull(ipDetails2);
            Assert.IsNotNull(ipDetails1);

            ipDetails1.Country = "Chernarus";
            ipDetails2.Country = "Chernarus";
            ipDetails3.Country = "Chernarus";
            var updateItems = new List<IPDetails>();
            updateItems.Add(ipDetails3);
            updateItems.Add(ipDetails2);
            updateItems.Add(ipDetails1);
            var jobGuid = serviceIPDetails.AddBatchJob(new UpdateJob(updateItems.Count));
            await serviceIPDetails.BatchUpdate(updateItems,jobGuid);

            var ips = new string[] { ip1, ip2, ip3 };

            foreach(var ip in ips)
            {
                var ipDetailsFromCache = repoCache.GetIPDetailsFromCache(ip);
                Assert.IsNotNull(ipDetailsFromCache);
                var ipDetailsFromDb = await repoIPDetailsDB.GetIPDetailsFromDB(ip);
                Assert.IsNotNull(ipDetailsFromDb);

                Assert.IsTrue(ipDetailsFromCache.Ip == ip && ipDetailsFromCache.Country == "Chernarus");
                Assert.IsTrue(ipDetailsFromDb.Ip == ip && ipDetailsFromDb.Country == "Chernarus");
            }
        }

    }
}
