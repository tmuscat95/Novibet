using Novibet.Library.Models;

namespace Novibet.API.Services.Interfaces
{
    public interface IServiceIPDetails
    {
        Task<IPDetails?> GetIPDetails(string ip);
    }
}
