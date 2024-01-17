using AffiliatePMS.Application.AffiliateCustomers.ListCustomers;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Infra.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _AffiliatePMS.WebAPI.Areas.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = Role.Affiliate)]
    public class CustomersController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Returns the total of customers reffered by the affiliate
        /// </summary>
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Head([FromServices] IIdentifierService identifierService)
        {
            var count = await mediator.Send(new GetTotalCustomersQuery() { AffiliateId = identifierService.AffiliateId!.Value });
            Response.Headers.Append("X-Total-Count", count.ToString());
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType<List<AffiliateCustomerResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromServices] IIdentifierService identifierService)
        {
            var data = await mediator.Send(new GetCustomersQuery() { AffiliateId = identifierService.AffiliateId!.Value });
            return Ok(data);
        }


    }
}
