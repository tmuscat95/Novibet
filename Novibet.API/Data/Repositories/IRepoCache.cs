using Novibet.Library.Models;

namespace Novibet.API.Data.Repositories
{
    public interface IRepoCache
    {
        IPDetails? GetIPDetailsFromCache(string ip);
        void InsertIPDetailsIntoCache(IPDetails iPDetails);

    }
}
