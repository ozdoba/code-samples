using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdatePrivateEmail
{
    public class UpdatePrivateEmailCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// The employees private email address
        /// </summary>
        public string PrivateEmail { get; set; }
    }

    public class UpdatePrivateEmailCommandHandler : IRequestHandler<UpdatePrivateEmailCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdatePrivateEmailCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdatePrivateEmailCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }
            
            employee.PrivateEmailAddress = command.PrivateEmail;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}