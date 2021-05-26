using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Employees.Queries.ExportEmployees
{
    public class ExportEmployeesQuery : IRequest<ExportEmployeesVm>
    {
        public Guid CustomerId { get; set; }
    }

    public class ExportEmployeesQueryHandler : IRequestHandler<ExportEmployeesQuery, ExportEmployeesVm>
    {
        private readonly IEmployeesContext _context;
        private readonly IMapper _mapper;
        private readonly ICsvFileBuilder _fileBuilder;

        public ExportEmployeesQueryHandler(IEmployeesContext context, IMapper mapper, ICsvFileBuilder fileBuilder)
        {
            _context = context;
            _mapper = mapper;
            _fileBuilder = fileBuilder;
        }
        
        public async Task<ExportEmployeesVm> Handle(ExportEmployeesQuery request, CancellationToken cancellationToken)
        {
            var vm = new ExportEmployeesVm();

            var records = await _context.Employees
                .AsNoTracking()
                .Where(e => e.CustomerId == request.CustomerId)
                .ProjectTo<EmployeeRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            vm.Content = _fileBuilder.BuildEmployeesFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = $"Employees-{request.CustomerId}.csv";

            return await Task.FromResult(vm);
        }
    }
}