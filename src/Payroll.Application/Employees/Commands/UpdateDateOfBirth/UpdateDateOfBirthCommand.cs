using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdateDateOfBirth
{
    public class UpdateDateOfBirthCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        
        /// <summary>
        /// The employees date of birth, format 1979-10-24 ("yyyy-MM-dd")
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateDateOfBirthCommandHandler : IRequestHandler<UpdateDateOfBirthCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateDateOfBirthCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdateDateOfBirthCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }
            
            employee.DateOfBirth = command.DateOfBirth;
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}