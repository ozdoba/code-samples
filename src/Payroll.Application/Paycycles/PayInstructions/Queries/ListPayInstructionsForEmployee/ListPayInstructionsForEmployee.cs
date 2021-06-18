using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.PayInstructions.Queries.Dto;

namespace Payroll.Application.Paycycles.PayInstructions.Queries.ListPayInstructionsForEmployee
{
    public class ListPayInstructionsForEmployee : IRequest<IEnumerable<InstructionDetailsDto>>
    {
        public Guid CustomerId { get; set; }
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class ListPayInstructionsForEmployeeHandler : IRequestHandler<ListPayInstructionsForEmployee, IEnumerable<InstructionDetailsDto>>
    {
        private readonly IPaycyclesContext _context;
        private readonly IMapper _mapper;

        public ListPayInstructionsForEmployeeHandler(IPaycyclesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        } 
        
        public async Task<IEnumerable<InstructionDetailsDto>> Handle(ListPayInstructionsForEmployee request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .AsNoTracking()
                .Include(x => x.Payees)
                .ThenInclude(x=>x.PayInstructions)
                .ThenInclude(x=>x.PayCode)
                .Where(x => x.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(x => x.PaycycleId == request.PaycycleId, cancellationToken);
            
            if (paycycle == default)
            {
                throw new PaycycleNotFound($"Paycycle [{request.PaycycleId}] not found");
            }

            var payee = paycycle.FindPayee(request.EmployeeNumber);

            if (payee == default)
            {
                throw new EmployeeNotFound($"Employee [{request.EmployeeNumber}] not in specified payroll");
            }

            var instructions = payee.PayInstructions
                .Select(x => _mapper.Map<InstructionDetailsDto>(x));

            return instructions;
        }
    }
}