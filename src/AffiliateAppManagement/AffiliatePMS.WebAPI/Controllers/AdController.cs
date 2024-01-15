using AffiliatePMS.Application.Ad;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Infra.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _AffiliatePMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Role.Affiliate)]
    public class AdController(IMediator mediator) : ControllerBase
    {

        [HttpGet("RealTimeStats")]
        [ProducesResponseType<List<AdRealTimeStats>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCustomers([FromServices] IIdentifierService identifier)
        {
            var data = await mediator.Send(new AdStatsQuery() { AffiliateId = identifier.AffiliateId!.Value });
            return Ok(data);
        }
    }
}
