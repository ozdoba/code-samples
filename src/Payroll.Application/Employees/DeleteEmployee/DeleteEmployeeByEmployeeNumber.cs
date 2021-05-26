using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.DeleteEmployee
{
    public class DeleteEmployeeByEmployeeNumber : IRequest
    {
        internal Guid CustomerId { get; set; }
        /// <summary>
        /// Your unique employee/staff identifier to be referenced when making subsequent payroll submission(s).
        /// </summary>
        public string EmployeeNumber { get; set; }
    }

    public class DeleteEmployeeByEmployeeNumberHandler : IRequestHandler<DeleteEmployeeByEmployeeNumber>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public DeleteEmployeeByEmployeeNumberHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(DeleteEmployeeByEmployeeNumber request, CancellationToken cancellationToken)
        {
            var item = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber, cancellationToken);
            
            if (item == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(request.EmployeeNumber);
            }
            
            _context.Employees.Remove(item);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}