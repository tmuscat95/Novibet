using Novibet.Library.Models;

namespace Novibet.API.Data.Repositories
{
    public interface IRepoIPDetailsDB
    {
        Task<bool> InsertIPDetailsIntoDB(IPDetails ipDetails);
        Task<bool> UpdateIPDetailsInDB(IPDetails iPDetails);
        Task<IPDetails?> GetIPDetailsFromDB(string ip);
    }
}
