using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Common.Mappings;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees.Queries.GetEmployeeDetails
{
    public class GetEmployeeDetailsQuery : IRequest<EmployeeDetailsType>
    {
        public string EmployeeNumber { get; set; }
    }

    public class GetEmployeeDetailsQueryHandler : IRequestHandler<GetEmployeeDetailsQuery, EmployeeDetailsType>
    {
        private readonly IEmployeesContext _context;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;

        public GetEmployeeDetailsQueryHandler(IEmployeesContext context, IMapper mapper, ICustomerService customerService)
        {
            _context = context;
            _mapper = mapper;
            _customerService = customerService;
        }
        
        public async Task<EmployeeDetailsType> Handle(GetEmployeeDetailsQuery request, CancellationToken cancellationToken)
        {
            var details = await _context.Employees
                .AsNoTracking()
                .Where(e=> e.CustomerId ==_customerService.GetCustomerId())
                .Where(e=>e.EmployeeNumber == request.EmployeeNumber)
                .ProjectTo<EmployeeDetailsType>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return details ?? throw EmployeeNotFound.ForEmployeeNumber(request.EmployeeNumber);
        }
    }
    
    public class EmployeeDetailsType : IMapFrom<Employee>
    {
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }

        public string JobTitle { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public AddressType Address { get; set; }

        public string PrivateEmailAddress { get; set; }
        public string CorporateEmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? LocalTaxNumber { get; set; }

        public DateTime? DateOfTermination { get; set; }

        public DateTime? DateOfEmployment { get; set; }
        public Employee.EmployeeStatus Status { get; set; }
        
        
    }

    public class AddressType : IMapFrom<Address>
    {
        public string BuildingNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}