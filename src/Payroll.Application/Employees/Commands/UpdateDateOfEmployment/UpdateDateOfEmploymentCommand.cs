using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdateDateOfEmployment
{
    public class UpdateDateOfEmploymentCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// The employees date of birth, format 1979-10-24 ("yyyy-MM-dd")
        /// </summary>
        // [SwaggerSchema(Format = "date")]
        public DateTime DateOfEmployment { get; set; }
    }

    public class UpdateDateOfEmploymentHandler : IRequestHandler<UpdateDateOfEmploymentCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateDateOfEmploymentHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdateDateOfEmploymentCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }
            
            employee.DateOfEmployment = command.DateOfEmployment;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}