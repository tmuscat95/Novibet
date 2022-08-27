using Microsoft.EntityFrameworkCore;
using Novibet.API.Data;
using Novibet.API.Services.Interfaces;
using Novibet.Library.Exceptions;
using Novibet.Library.Interfaces;
using Novibet.Library.Models;
using System.Runtime.Caching;

namespace Novibet.API.Services
{
    public class ServiceIPDetails : IServiceIPDetails
    {
        private readonly IPDetailsDBContext context;
        private readonly IIPInfoProvider ipInfoProvider;
        private CacheItemPolicy policy;
        private ObjectCache cache = MemoryCache.Default;
        public ServiceIPDetails(IPDetailsDBContext iPDetailsDBContext, IIPInfoProvider provider)
        {
            context = iPDetailsDBContext;
            ipInfoProvider = provider;
            policy = new CacheItemPolicy();
        }

        private async Task<IPDetails?> GetIPDetailsFromDB(string ip)
        {
            return await context.Ipdetails.Where(r => r.Ip == ip).Select(r => r).FirstOrDefaultAsync();
        }

        private IPDetails? GetIPDetailsFromCache(string ip)
        {
            return (IPDetails?)cache.Get(ip);
        }

        private IPDetails GetIPDetailsFromAPI(string ip)
        {
            return ipInfoProvider.GetDetails(ip);
        }
        public async Task<IPDetails?> GetIPDetails(string ip)
        {
            IPDetails? iPDetails;
            iPDetails = GetIPDetailsFromCache(ip);
            if (iPDetails == null)
            {
                iPDetails = await GetIPDetailsFromDB(ip);
                if (iPDetails != null)
                    InsertIPDetailsIntoCache(iPDetails);
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
                InsertIPDetailsIntoCache(iPDetails);
                await InsertIPDetailsIntoDB(iPDetails);
            }
            return iPDetails;
        }

        private async Task<bool> InsertIPDetailsIntoDB(IPDetails ipDetails)
        {
            try
            {
                await context.Ipdetails.AddAsync(ipDetails);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void InsertIPDetailsIntoCache(IPDetails iPDetails)
        {
            cache.Set(iPDetails.Ip, iPDetails, policy);
        }
    }
}
