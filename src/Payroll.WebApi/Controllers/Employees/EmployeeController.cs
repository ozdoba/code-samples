using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Employees.Commands.AddIdDocument;
using Payroll.Application.Employees.Commands.DeleteEmployee;
using Payroll.Application.Employees.Commands.RegisterEmployee;
using Payroll.Application.Employees.Commands.UpdateEmployee;
using Payroll.Application.Employees.Queries.ExportEmployees;
using Payroll.Application.Employees.Queries.GetEmployeeDetails;
using Payroll.Application.Employees.Queries.ListIdDocuments;

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
        private readonly ICustomerService _customerService;

        public EmployeesController(IMediator mediator, ICustomerService customerService)
        {
            _mediator = mediator;
            _customerService = customerService;
        }
        
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
        /// Get employee details
        /// </summary>
        /// <remarks>
        /// Get details of an employee based on the employeeId
        /// </remarks>
        /// <param name="employeeNumber">Your unique employee/staff identifier</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{employeeNumber}")]
        // [SwaggerOperation(Tags = new []{ "Employees" })]
        [ProducesResponseType(typeof(EmployeeDetailsType), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<EmployeeDetailsType>> FindByEmployeeByNumber([FromRoute] string employeeNumber)
        {
            return await _mediator.Send(new GetEmployeeDetailsQuery() { EmployeeNumber = employeeNumber });
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
        
        
        [HttpGet("{customerId}/export")]
        public async Task<FileResult> Get([FromRoute] Guid customerId)
        {
            var vm = await _mediator.Send(new ExportEmployeesQuery { CustomerId = customerId });

            return File(vm.Content, vm.ContentType, vm.FileName);
        }
        
        
        /// <summary>
        /// Upload a new ID document
        /// </summary>
        /// <remarks>
        /// Upload a new ID document for the specified employee
        /// </remarks>
        /// <param name="employeeNumber">Your unique employee/staff identifier</param>
        /// <param name="command">Payload containing details of the new id document</param>
        /// <returns></returns>
        [HttpPost]
        [Route("idDocuments")]
        // [SwaggerOperation(Tags = new []{ "Employees" })]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddIdDocument(
            [FromBody] AddIdDocumentCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        
        /// <summary>
        /// List the id documents for the given employee
        /// </summary>
        /// <remarks>
        /// List the id documents for the given employee
        /// </remarks>
        /// <param name="employeeNumber">Your unique employee/staff identifier</param>
        /// <param name="command">Payload containing details of the new id document</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{employeeNumber}/idDocuments")]
        // [SwaggerOperation(Tags = new []{ "Employees" })]
        [ProducesResponseType(typeof(IEnumerable<IdDocumentsListVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListIdDocuments([FromRoute] string employeeNumber)
        {
            var idDocuments = await _mediator.Send(new ListIdDocumentsQuery { CustomerId = _customerService.GetCustomerId(), EmployeeNumber = employeeNumber });
            return new OkObjectResult(idDocuments);
        }
    }
}