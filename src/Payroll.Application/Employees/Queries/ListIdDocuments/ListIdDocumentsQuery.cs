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

namespace Payroll.Application.Employees.Queries.ListIdDocuments
{
    public class ListIdDocumentsQuery : IRequest<IdDocumentsListVm>
    {
        public Guid CustomerId { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class ListIdDocumentsQueryHandler : IRequestHandler<ListIdDocumentsQuery, IdDocumentsListVm>
    {
        private readonly IEmployeesContext _context;
        private readonly IMapper _mapper;

        public ListIdDocumentsQueryHandler(IEmployeesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IdDocumentsListVm> Handle(ListIdDocumentsQuery request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.CustomerId == request.CustomerId)
                .SingleOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber, cancellationToken);
        
            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(request.EmployeeNumber);
            }

            return new IdDocumentsListVm
            {
                EmployeeNumber = employee.EmployeeNumber,
                Documents = await _context.IdDocuments
                    .AsNoTracking()
                    .ProjectTo<IdDocumentType>(_mapper.ConfigurationProvider)
                    .OrderByDescending(t => t.ExpiryDate)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}