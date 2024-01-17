using _AffiliatePMS.WebAPI._Common;
using AffiliatePMS.Application.Affiliates.Create;
using AffiliatePMS.Application.Affiliates.Query;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Infra.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _AffiliatePMS.WebAPI.Areas.Admin
{
    [Tags("Admin Affiliate")]
    [Route("api/v1/admin/[controller]")]
    [Authorize(Roles = Role.Admin)]
    [ApiController]
    public class AffiliatesController(IMediator mediator, ILogger<AffiliatesController> logger) : ControllerBase
    {

        /// <summary>
        /// Returns the total of affiliates
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Head()
        {
            var count = await mediator.Send(new GetAffiliateHeadQuery());
            Response.Headers.Append("X-Total-Count", count.ToString());
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType<List<AffiliateResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromQuery] Pagination pagination)
        {
            var data = await mediator.Send(new GetAffiliateQuery());
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateAffiliateCommand command)
        {
            try
            {
                var response = await mediator.Send(command);
                return response.Match(
                 _ => CreatedAtAction(nameof(Get), new { id = response.Data!.Id }, response.Data),
                 _ => Problem(response.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on adding affiliate {name}", command.PublicName);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType<AffiliateResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await mediator.Send(new GetAffiliateByIdQuery(id));
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);

        }


    }
}
