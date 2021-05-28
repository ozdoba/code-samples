using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdateMobileNumber
{
    public class UpdateMobileNumberCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        
        /// <summary>
        /// An international phone number in I.164 format, i.e. '+442071838750'
        /// </summary>
        public string MobileNumber { get; set; }
    }

    public class UpdateMobileNumberCommandHandler : IRequestHandler<UpdateMobileNumberCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateMobileNumberCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        public async Task<Unit> Handle(UpdateMobileNumberCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }
            
            employee.MobileNumber = command.MobileNumber;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}