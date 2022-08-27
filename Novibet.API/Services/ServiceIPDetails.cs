using Novibet.API.Data.Repositories;
using Novibet.API.Types.Interfaces;
using Novibet.Library.Exceptions;
using Novibet.Library.Interfaces;
using Novibet.Library.Models;

namespace Novibet.API.Types
{
    public partial class ServiceIPDetails : IServiceIPDetails
    {
        private readonly IIPInfoProvider ipInfoProvider;
        private readonly IRepoIPDetailsDB repoIPDetailsDB;
        private readonly IRepoCache repoCache;

        private static Dictionary<Guid, UpdateJob> Jobs = new Dictionary<Guid, UpdateJob>();

        public static UpdateJob GetJob(Guid guid)
        {
            return Jobs[guid];
        }
        public ServiceIPDetails(IRepoCache repoCache, IRepoIPDetailsDB repoIPDetailsDB, IIPInfoProvider ipInfoProvider)
        {
            this.repoIPDetailsDB = repoIPDetailsDB;
            this.ipInfoProvider = ipInfoProvider;
            this.repoCache = repoCache;
        }
        private IPDetails GetIPDetailsFromAPI(string ip)
        {
            return ipInfoProvider.GetDetails(ip);
        }
        public async Task<IPDetails?> GetIPDetails(string ip)
        {
            IPDetails? iPDetails;
            iPDetails = repoCache.GetIPDetailsFromCache(ip);
            if (iPDetails == null)
            {
                iPDetails = await repoIPDetailsDB.GetIPDetailsFromDB(ip);
                if (iPDetails != null)
                    repoCache.InsertIPDetailsIntoCache(iPDetails);
            }
            if (iPDetails == null)
            {
                try
                {
                    iPDetails = GetIPDetailsFromAPI(ip);
                }
                catch (IPServiceNotAvailableException e)
                {
                    throw e;
                }
                repoCache.InsertIPDetailsIntoCache(iPDetails);
                await repoIPDetailsDB.InsertIPDetailsIntoDB(iPDetails);
            }
            return iPDetails;
        }
        public async Task Update(IPDetails iPDetails)
        {
            await repoIPDetailsDB.UpdateIPDetailsInDB(iPDetails);
            repoCache.InsertIPDetailsIntoCache(iPDetails);
        }
        public async Task BatchUpdate(IEnumerable<IPDetails> items, Guid jobGuid)
        {


            while (items.Any())
            {
                var tasks = new List<Task>();
                var n = 10;
                if (items.Count() < n)
                    n = items.Count();

                var batch10 = items.Take(n);
                items = items.Skip(n);
                foreach (var item in batch10)
                {
                    tasks.Add(Update(item));
                }
                await Task.WhenAll(tasks);
                Jobs[jobGuid].itemsDone += n;
            }
        }

        public Guid AddBatchJob(UpdateJob job)
        {
            var guid = Guid.NewGuid();
            Jobs.Add(guid, job);
            return guid;
        }

        public string GetJobProgress(Guid jobGuid)
        {
            if (!Jobs.ContainsKey(jobGuid))
                return String.Empty;
            return Jobs[jobGuid].GetDoneRatio();
        }
    }
}
