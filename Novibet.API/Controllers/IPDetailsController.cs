using Microsoft.AspNetCore.Mvc;
using Novibet.API.Types.Interfaces;
using Novibet.Library.Models;

namespace Novibet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IPDetailsController : ControllerBase
    {
        private readonly ILogger<IPDetailsController> _logger;
        private readonly IServiceIPDetails _serviceIPDetails;
        public IPDetailsController(ILogger<IPDetailsController> logger, IServiceIPDetails serviceIPDetails)
        {
            _logger = logger;
            _serviceIPDetails = serviceIPDetails;
            
        }

        [HttpGet]
        public async Task<ActionResult<IPDetails?>> Get([FromQuery] string ip)
        {
            try
            {
                var ipDetails = await _serviceIPDetails.GetIPDetails(ip);
                return Ok(ipDetails);
            }catch(Exception e)
            {
                return StatusCode(500,e.Message);
            }
            
        }

        [HttpPost("/batch_update/new")]
        public Guid BatchUpdate([FromBody] List<IPDetails> updateItems)
        {
            var taskGuid = _serviceIPDetails.AddBatchJob(new Types.UpdateJob(updateItems.Count));
            _serviceIPDetails.BatchUpdate(updateItems,taskGuid);
            
            return taskGuid;
        }

        [HttpGet("/batch_update/progress")]
        public ActionResult<string> GetProgress([FromQuery] Guid guid)
        {

            var progress = _serviceIPDetails.GetJobProgress(guid);
            if(progress == String.Empty)
            {
                return NotFound("No such job");
            }

            return Ok(progress.ToString());
        }
    }
}