using Novibet.Library;
using Novibet.Library.Interfaces;

namespace Novibet.Tests
{
    public class LibraryTests
    {
        IIPInfoProvider provider;
        [SetUp]
        public void Setup()
        {
            this.provider = new IPInfoProvider();
        }

        [Test]
        [Description("Tests whether library can retrieve known data for a given ip address. (Google Public DNS)")]
        public void ApiCallTest()
        {
            var ip = "8.8.8.8";
            var ipDetails = provider.GetDetails(ip);
            Assert.That(ipDetails, Is.Not.Null);
            Assert.IsTrue(ipDetails.Country == "United States");
            Assert.IsTrue(ipDetails.Ip == ip);
            Assert.IsTrue(ipDetails.Continent == "North America");
            Assert.IsTrue(ipDetails.City == "Glenmont");
        }
    }
}
