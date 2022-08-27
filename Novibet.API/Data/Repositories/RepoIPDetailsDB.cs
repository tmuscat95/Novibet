using Microsoft.EntityFrameworkCore;
using Novibet.Library.Models;

namespace Novibet.API.Data.Repositories
{
    public class RepoIPDetailsDB : IRepoIPDetailsDB
    {
        private readonly IPDetailsDBContext context;

        public RepoIPDetailsDB(IPDetailsDBContext iPDetailsDBContext)
        {
            context = iPDetailsDBContext;

        }

        public void TruncateIPDetailsTable()
        {
            context.Ipdetails.RemoveRange(context.Ipdetails.Select(x=>x).ToList());
            context.SaveChanges();
        }
        public async Task<IPDetails?> GetIPDetailsFromDB(string ip)
        {
            return await context.Ipdetails.Where(r => r.Ip == ip).Select(r => r).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertIPDetailsIntoDB(IPDetails ipDetails)
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

        public async Task<bool> UpdateIPDetailsInDB(IPDetails iPDetails)
        {
            try
            {
                var row = await context.Ipdetails.Where(x => x.Ip == iPDetails.Ip).FirstOrDefaultAsync();
                if (row != null)
                {

                    row.Longitude = iPDetails.Longitude;
                    row.Latitude = iPDetails.Latitude;
                    row.City = iPDetails.City;
                    row.Continent = iPDetails.Continent;
                    row.Country = iPDetails.Country;
                    await context.SaveChangesAsync();

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
