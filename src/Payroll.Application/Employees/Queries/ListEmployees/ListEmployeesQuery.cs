using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees.Queries.ListEmployees
{
    public class ListEmployeesQuery : IRequest<ListEmployeesResponse>
    {
        public EmployeeStatusType? Status { get; set; }
    }

    public class ListEmployeesQueryHandler : IRequestHandler<ListEmployeesQuery, ListEmployeesResponse>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public ListEmployeesQueryHandler(IEmployeesContext context, ICustomerService customerService, IMapper mapper)
        {
            _context = context;
            _customerService = customerService;
            _mapper = mapper;
        }
        
        public async Task<ListEmployeesResponse> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
        {
            var customerId = _customerService.GetCustomerId();
            
            var employeeFilter = _context.Employees
                .Where(e => e.CustomerId == customerId);
            
            if (request.Status.HasValue)
            {
                var employeeStatus = Enum.Parse<Employee.EmployeeStatus>(request.Status.Value.ToString());
                employeeFilter = employeeFilter
                    .Where(e => e.Status == employeeStatus);
            }

            return new ListEmployeesResponse
            {
                CustomerId = customerId,
                Employees = await employeeFilter
                    .AsNoTracking()
                    .ProjectTo<EmployeesListItemType>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
                    
            };
        }
    }
}