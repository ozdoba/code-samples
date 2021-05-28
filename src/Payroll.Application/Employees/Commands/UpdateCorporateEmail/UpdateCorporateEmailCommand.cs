using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdateCorporateEmail
{
    public class UpdateCorporateEmailCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// The employees corporate email address
        /// </summary>
        public string CorporateEmail { get; set; }
    }

    public class UpdateCorporateEmailCommandHandler : IRequestHandler<UpdateCorporateEmailCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateCorporateEmailCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdateCorporateEmailCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }

            employee.CorporateEmailAddress = command.CorporateEmail;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}