using Microsoft.AspNetCore.Mvc;
using Novibet.API.Services.Interfaces;
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
    }
}