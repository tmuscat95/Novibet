using Novibet.Library.Models;
using Novibet.API.Types;
namespace Novibet.API.Types.Interfaces
{
    public interface IServiceIPDetails
    {
        Task<IPDetails?> GetIPDetails(string ip);
        Task BatchUpdate(IEnumerable<IPDetails> items, Guid jobGuid);
        Guid AddBatchJob(UpdateJob job);

        string GetJobProgress(Guid jobGuid);
    }
}
