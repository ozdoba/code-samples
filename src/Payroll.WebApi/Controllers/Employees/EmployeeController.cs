using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.Employees.DeleteEmployee;
using Payroll.Application.Employees.RegisterEmployee;
using Payroll.Application.Employees.UpdateEmployee;

namespace Payroll.WebApi.Controllers.Employees
{
    /// <summary>
    /// Employee endpoints
    /// </summary>
    /// <remarks>
    /// What to do with employees?
    /// </remarks>
    [ApiController]
    [Route("payroll/employees/v1")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)] 
    [ApiConventionType(typeof(DefaultApiConventions))]
    // [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator) => _mediator = mediator;
        
        /// <summary>
        /// Registers a new employee
        /// </summary>
        /// <remarks>
        /// Registers a new employee
        /// </remarks>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        // [SwaggerOperation(Tags = new []{ "Employees" })]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<string>> Register(RegisterEmployeeCommand command)
        {
            return await _mediator.Send(command);
        }
        
        /// <summary>
        /// Delete employee
        /// </summary>
        /// <remarks>
        /// The specified employee is marked as deleted, but not removed from the system.
        /// It's data will be obfuscated in order to comply with GDPR rules.
        ///
        /// This operation will cascade to associated IdDocument records.
        /// </remarks>
        /// <param name="employeeNumber">Your unique employee/staff identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{employeeNumber}")]
        // [SwaggerOperation(Tags = new []{ "Employees" })]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult> DeleteByEmployeeNumber([FromRoute] string employeeNumber)
        {
            await _mediator.Send(new DeleteEmployeeByEmployeeNumber {EmployeeNumber = employeeNumber});
            return Ok();
        }
        
        /// <summary>
        /// Update employee details
        /// </summary>
        /// <remarks>
        /// Update the details of an employee
        /// </remarks>
        /// <param name="employeeNumber">Your unique employee/staff identifier</param>
        /// <param name="command">Payload containing details of the employee</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{employeeNumber}/details")]
        // [SwaggerOperation(Tags = new []{ "Employees" })]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateEmployeeDetails(
            [FromRoute] string employeeNumber, 
            [FromBody] UpdateEmployeeDetailsCommand command)
        {
            command.EmployeeNumber = employeeNumber;
            await _mediator.Send(command);
            return Ok();
        }
    }
}