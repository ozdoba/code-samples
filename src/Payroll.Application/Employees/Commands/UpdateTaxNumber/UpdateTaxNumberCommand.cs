using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdateTaxNumber
{
    public class UpdateTaxNumberCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        
        /// <summary>
        /// The employees local tax number
        /// </summary>
        public string LocalTaxNumber { get; set; }
    }

    public class UpdateTaxNumberCommandHandler : IRequestHandler<UpdateTaxNumberCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateTaxNumberCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdateTaxNumberCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw new EmployeeNotFound($"Employee [{request.EmployeeNumber}] not found.");
            }
            
            employee.LocalTaxNumber = request.LocalTaxNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}