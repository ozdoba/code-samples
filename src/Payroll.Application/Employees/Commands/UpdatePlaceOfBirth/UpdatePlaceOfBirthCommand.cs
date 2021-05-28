using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Commands.UpdatePlaceOfBirth
{
    public class UpdatePlaceOfBirthCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// The employees place of birth
        /// </summary>
        public string PlaceOfBirth { get; set; }
    }

    public class UpdatePlaceOfBirthCommandHandler : IRequestHandler<UpdatePlaceOfBirthCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdatePlaceOfBirthCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        public async Task<Unit> Handle(UpdatePlaceOfBirthCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(request.EmployeeNumber);
            }
            
            employee.PlaceOfBirth = request.PlaceOfBirth;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}