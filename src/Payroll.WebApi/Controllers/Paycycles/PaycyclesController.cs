using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.Paycycles.General.Commands.ConfirmPaycycle;
using Payroll.Application.Paycycles.General.Commands.CreatePaycycle;
using Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails;

namespace Payroll.WebApi.Controllers.Paycycles
{
    /// <summary>
    /// A paycycle describes a payment period within the context of the payroll.
    ///
    /// Transactions within this period can be payments or deductions, and form a sequence that is used to calculate the
    /// amount of income that is to be transfered to the employer on the payday of the paycycle.
    ///
    /// A paycycle has a defined start-date and end-date, i.e. "2021-03-05 -- 2021-04-04", and a defined paydate (the
    /// date the money will be transferred to the employee).
    /// </summary>
    [ApiController]
    [Route("payroll/paycycles/v1")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)] 
    [ApiConventionType(typeof(DefaultApiConventions))]
    // [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class PaycyclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaycyclesController(IMediator mediator) => _mediator = mediator;
        
        
        /// <summary>
        /// Create a new paycycle
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        // [SwaggerOperation(Tags = new []{ "Paycycle" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreatePaycycle([FromBody] CreatePaycycleCommand command)
        {
            return new OkObjectResult(await _mediator.Send(command));
        }
        
        
        /// <summary>
        /// Show the details of the paycycle
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{paycycleId}")]
        // [SwaggerOperation(Tags = new[] {"Paycycle"})]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(PaycycleDetailsVm))]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetPaycycleDetails([FromRoute] Guid paycycleId)
        {
            return new OkObjectResult(await _mediator.Send(new GetPaycycleDetailsQuery { PaycycleId = paycycleId }));
        }
        
        /// <summary>
        /// Confirms the Paycycle. Once confirmed, the pay cycle will be locked and no more additions or removals are possible.
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{paycycleId}/confirm")]
        // [SwaggerOperation(Tags = new[] {"Paycycle"})]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ConfirmPaycycle([FromRoute] Guid paycycleId)
        {
            await _mediator.Send(new ConfirmPaycycleCommand { PaycycleId = paycycleId });
            return NoContent();
        }
    }
}