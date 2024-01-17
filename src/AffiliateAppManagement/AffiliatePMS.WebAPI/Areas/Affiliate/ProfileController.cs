using _AffiliatePMS.WebAPI._Common;
using AffiliatePMS.Application.Affiliates.Create;
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
    public class ProfileController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = Role.Incomplete)]
        [ProducesResponseType<List<AffiliateCustomerResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(CreateAffiliateProfileCommand createProfile)
        {
            var response = await mediator.Send(createProfile);
            return response.Match(
                    _ => Ok(response),
                    _ => Problem(response.Message));
        }
    }
}
