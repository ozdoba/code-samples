using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.Paycycles.PayCodes.Commands.AddPayCode;
using Payroll.Application.Paycycles.PayCodes.Commands.DeletePayCode;
using Payroll.Application.Paycycles.PayCodes.Queries.ListPayCodes;
using Swashbuckle.AspNetCore.Annotations;

namespace Payroll.WebApi.Controllers.Paycycles
{
    [ApiController]
    [Route("payroll/paycycles/v1/paycodes")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)] 
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class PayCodesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayCodesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>List available PayCodes</summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] {"PayCode"})]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(ListPayCodesResponse))]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListPayCodes()
        {
            return Ok(await _mediator.Send(new ListPayCodesQuery()));
        }
        
        /// <summary>Add PayCode</summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] {"PayCode"})]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddPayCode([FromBody] AddPayCodeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
        /// <summary>Delete PayCode</summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{code}")]
        [SwaggerOperation(Tags = new[] {"PayCode"})]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeletePayCode([FromRoute] string code)
        {
            await _mediator.Send(new DeletePayCodeCommand {Code = code});
            return NoContent();
        }

    }
}