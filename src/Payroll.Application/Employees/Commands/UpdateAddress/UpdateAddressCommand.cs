using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Employees.Commands.Shared;

namespace Payroll.Application.Employees.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// The new Address
        /// </summary>
        public Address Address { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateAddressCommandHandler(IEmployeesContext context, ICustomerService customerService)
        { 
            _context = context;
            _customerService = customerService;
        }

        public async Task<Unit> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }

            employee.Address.BuildingNumber = command.Address.BuildingNumber;
            employee.Address.Street = command.Address.Street;
            employee.Address.City = command.Address.City;
            employee.Address.State = command.Address.State;
            employee.Address.PostalCode = command.Address.PostalCode;
            employee.Address.CountryCode = command.Address.CountryCode;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
            ;
        }
    }
}