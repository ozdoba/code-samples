using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;

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
}