using _AffiliatePMS.WebAPI._Common;
using AffiliatePMS.Application.AffiliateCustomers.CreateCustomer;
using AffiliatePMS.Application.AffiliateCustomers.ListCustomers;
using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Infra.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _AffiliatePMS.WebAPI.Areas.Admin
{
    [Tags("Admin Affiliate")]
    [Route("api/v1/admin/affiliates/{id}/[controller]")]
    [Authorize(Roles = Role.Admin)]
    [ApiController]
    public class CustomersController(IMediator mediator, ILogger<CustomersController> logger) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([Required] int id, [FromBody] CreateAffiliateCustomerCommand command)
        {
            try
            {
                var response = await mediator.Send(command);
                return response.Match(
                 _ => CreatedAtAction(nameof(Get), new { id, customerid = response!.Data!.Id }, response.Data),
                 _ => Problem(response.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on adding affiliate {name}", command.FullName);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType<AffiliateCustomerResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, int customerId)
        {
            var data = await mediator.Send(new GetCustomerByIdQuery() { CustomerId = customerId });
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType<List<AffiliateCustomerResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCustomers(int id)
        {
            var data = await mediator.Send(new GetCustomersQuery() { AffiliateId = id });
            return Ok(data);
        }
    }
}
