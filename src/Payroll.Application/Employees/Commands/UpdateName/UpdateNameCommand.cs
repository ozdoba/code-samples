using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdateName
{
    public class UpdateNameCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        
        /// <summary>
        /// The employees first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The employees middle name (if applicable)
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// The employees last name
        /// </summary>
        public string LastName { get; set; }
    }

    public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateNameCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        public async Task<Unit> Handle(UpdateNameCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }

            employee.FirstName = command.FirstName;
            employee.MiddleName = command.MiddleName;
            employee.LastName = command.LastName;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}