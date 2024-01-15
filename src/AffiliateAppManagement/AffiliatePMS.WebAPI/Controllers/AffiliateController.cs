using AffiliatePMS.Application.AffiliateCustomers.ListCustomers;
using AffiliatePMS.Application.Affiliates.Create;
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
    public class AffiliateController(IMediator mediator) : ControllerBase
    {
        [HttpPost("createAffiliateProfile")]
        [Authorize(Roles = Role.Incomplete)]
        [ProducesResponseType<List<AffiliateCustomerResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompleteProfile(CreateAffiliateCommand affiliateCommand,
            [FromServices] IIdentifierService identifierService)
        {
            var data = await mediator.Send(affiliateCommand);
            return Ok(data);
        }

        [HttpGet("customers")]
        [ProducesResponseType<List<AffiliateCustomerResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCustomers([FromServices] IIdentifierService identifierService)
        {
            var data = await mediator.Send(new GetCustomersQuery() { AffiliateId = identifierService.AffiliateId!.Value });
            return Ok(data);
        }

        /// <summary>
        /// Returns the total of customers reffered by the affiliate
        /// </summary>
        /// <param name="identifierService"></param>
        /// <returns></returns>
        [HttpHead("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Head([FromServices] IIdentifierService identifierService)
        {
            var count = await mediator.Send<int>(new GetTotalCustomersQuery() { AffiliateId = identifierService.AffiliateId!.Value });
            Response.Headers.Append("X-Total-Count", count.ToString());
            return Ok();
        }
    }
}
