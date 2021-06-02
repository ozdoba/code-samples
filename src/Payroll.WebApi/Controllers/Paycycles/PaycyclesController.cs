using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles;
using Payroll.Application.Paycycles.General.Commands.ConfirmPaycycle;
using Payroll.Application.Paycycles.General.Commands.CreatePaycycle;
using Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails;
using Payroll.Application.Paycycles.PayInstructions.Commands.AddPayInstruction;
using Payroll.Application.Paycycles.PayInstructions.Commands.RemovePayInstruction;
using Payroll.Application.Paycycles.PayInstructions.Commands.UpdatePayInstruction;
using Payroll.Application.Paycycles.PayInstructions.Queries.Dto;
using Payroll.Application.Paycycles.PayInstructions.Queries.GetInstructionDetailsQuery;
using Payroll.Application.Paycycles.PayInstructions.Queries.ListPayInstructionsForEmployee;
using Payroll.Application.Paycycles.PaymentOptions.Commands.UpdatePaymentOptions;
using Payroll.Application.Paycycles.PaymentOptions.Queries;

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
        private readonly ICustomerService _customerService;

        public PaycyclesController(ICustomerService customerService, IMediator mediator)
        {
            _customerService = customerService;
            _mediator = mediator;
        }
            

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
            return Ok(await _mediator.Send(command));
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
            return Ok(await _mediator.Send(new GetPaycycleDetailsQuery { PaycycleId = paycycleId }));
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
        
        
        
        
        
        /// <summary>
        /// Add pay instruction for an Employee
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{paycycleId}/{employeeNumber}/instructions")]
        // [SwaggerOperation(Tags = new []{ "Payinstruction" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddInstruction(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber,
            [FromBody] AddPayInstructionCommand command)
        {
            command.PaycycleId = paycycleId;
            command.EmployeeNumber = employeeNumber;
            return Ok(await _mediator.Send(command));
        }
        
        /// <summary>
        /// List the current pay instructions for an employee
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{paycycleId}/{employeeNumber}/instructions")]
        // [SwaggerOperation(Tags = new []{ "Payinstruction" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<InstructionDetailsDto>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListInstructions(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber)
        {
            return Ok(await _mediator.Send(new ListPayInstructionsForEmployee
            {
                CustomerId = _customerService.GetCustomerId(),
                PaycycleId = paycycleId,
                EmployeeNumber = employeeNumber,
            }));
        }
        
        
        /// <summary>
        /// Retrieve details for a specific pay instruction
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <param name="instructionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{paycycleId}/{employeeNumber}/instructions/{instructionId}")]
        // [SwaggerOperation(Tags = new []{ "Payinstruction" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetInstructionDetails(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber,
            [FromRoute] Guid instructionId)
        {
            return Ok(await _mediator.Send(new GetInstructionDetailsQuery
            {
                CustomerId = _customerService.GetCustomerId(),
                PaycycleId = paycycleId,
                EmployeeNumber = employeeNumber,
                InstructionId = instructionId,
            }));
        }


        

        
        
        
        /// <summary>
        /// Update an existing pay instruction for an Employee. This replaces the specified pay instruction.
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <param name="instructionId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{paycycleId}/{employeeNumber}/instructions/{instructionId}")]
        // [SwaggerOperation(Tags = new []{ "Payinstruction" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateInstructionDetails(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber,
            [FromRoute] Guid instructionId,
            [FromBody] UpdatePayInstructionCommand command)
        {
            command.PaycycleId = paycycleId;
            command.EmployeeNumber = employeeNumber;
            command.PayInstructionId = instructionId;
            await _mediator.Send(command);
            return NoContent();
        }
        
        
        /// <summary>
        /// Removes a pay instruction
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <param name="instructionId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{paycycleId}/{employeeNumber}/instructions/{instructionId}")]
        // [SwaggerOperation(Tags = new []{ "Payinstruction" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> RemoveInstruction(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber,
            [FromRoute] Guid instructionId)
        {
            await _mediator.Send(new RemovePayInstructionCommand()
            {
                CustomerId = _customerService.GetCustomerId(),
                PaycycleId = paycycleId,
                EmployeeNumber = employeeNumber,
                InstructionId = instructionId
            });

            return NoContent();
        }

        
        /// <summary>
        /// Returns the bank details for the specified employee
        /// </summary>
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{paycycleId}/{employeeNumber}/paymentOptions")]
        // [SwaggerOperation(Tags = new []{ "Payroll" })]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetPaymentOptions(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber)
        {
            return new OkObjectResult(await _mediator.Send(new GetPaymentOptionsQuery
            {
                CustomerId = _customerService.GetCustomerId(),
                PaycycleId = paycycleId,
                EmployeeNumber = employeeNumber
            }));
        }
        
        
        /// <summary>
        /// Update bank details, Creates the bank details if not yet set.
        /// </summary>
        /// This is some other stuff
        /// <param name="paycycleId"></param>
        /// <param name="employeeNumber"></param>
        /// <param name="paymentOptions"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{paycycleId}/{employeeNumber}/paymentOptions")]
        // [SwaggerOperation(Tags = new []{ "Payroll" })]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdatePaymentOptions(
            [FromRoute] Guid paycycleId,
            [FromRoute] string employeeNumber,
            [FromBody] UpdatePaymentOptionsCommand command)
        {
            command.PaycycleId = paycycleId;
            command.EmployeeNumber = employeeNumber;
            command.CustomerId = _customerService.GetCustomerId();
            
            await _mediator.Send(command);
            return NoContent();
        }
    }
}