using AffiliatePMS.Application.AffiliateCustomers.CreateCustomer;
using AffiliatePMS.Application.AffiliateCustomers.ListCustomers;
using AffiliatePMS.Application.Affiliates.Create;
using AffiliatePMS.Application.Affiliates.Query;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Infra.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _AffiliatePMS.WebAPI.Admin
{
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    [Route("api/admin/[controller]")]
    [Tags("Admin Operations")]
    public class AffiliateController(IMediator mediator, ILogger<AffiliateController> logger) : ControllerBase
    {

        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Head()
        {
            var count = await mediator.Send<int>(new GetAffiliateHeadQuery());
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
        public async Task<IActionResult> CreateAffiliate([FromBody] CreateAffiliateCommand command)
        {
            try
            {
                var response = await mediator.Send(command);
                if (response.IsSuccess)
                {
                    logger.LogInformation("Affiliates {id}:{name} added successfully ", response?.Data?.Id, command.PublicName);
                    return CreatedAtAction(nameof(GetAffiliated), new { id = response!.Data!.Id }, response);
                }
                return Conflict(response.Message);
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
        public async Task<IActionResult> GetAffiliated(int id)
        {
            var entity = await mediator.Send(new GetAffiliateByIdQuery(id));
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);

        }

        [HttpPost("{id}/customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer(int id, [FromBody] CreateAffiliateCustomerCommand command)
        {
            try
            {
                if (command.AffiliateId != id)
                {
                    return BadRequest("AffiliateId must be the same as the one in the URL");
                }

                var response = await mediator.Send(command);
                if (response.IsSuccess)
                {
                    logger.LogInformation("Affiliates {id}:{name} added successfully ", response?.Data?.Id, command.FullName);
                    return CreatedAtAction(nameof(GetCustomerById), new { id, customerid = response!.Data!.Id }, response);
                }
                return Conflict(response.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on adding affiliate {name}", command.FullName);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/customers/{customerId}")]
        [ProducesResponseType<AffiliateCustomerResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerById(int id, int customerId)
        {
            var data = await mediator.Send(new GetCustomerByIdQuery() { CustomerId = customerId });
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("{id}/customers")]
        [ProducesResponseType<List<AffiliateCustomerResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCustomers(int id)
        {
            var data = await mediator.Send(new GetCustomersQuery() { AffiliateId = id });
            return Ok(data);
        }
    }
}
