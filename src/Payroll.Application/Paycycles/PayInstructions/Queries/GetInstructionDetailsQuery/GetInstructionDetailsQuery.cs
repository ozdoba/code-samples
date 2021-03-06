using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.PayInstructions.Queries.Dto;

namespace Payroll.Application.Paycycles.PayInstructions.Queries.GetInstructionDetailsQuery
{
    public class GetInstructionDetailsQuery : IRequest<InstructionDetailsDto>
    {
        public Guid CustomerId { get; set; }
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
        public Guid InstructionId { get; set; }
    }

    public class GetInstructionDetailsQueryHandler : IRequestHandler<GetInstructionDetailsQuery, InstructionDetailsDto>
    {
        private readonly IPaycyclesContext _context;
        private readonly IMapper _mapper;

        public GetInstructionDetailsQueryHandler(IPaycyclesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<InstructionDetailsDto> Handle(GetInstructionDetailsQuery request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .AsNoTracking()
                .Include(x => x.Payees)
                .ThenInclude(x=>x.PayInstructions)
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

            var instruction = payee.FindInstruction(request.InstructionId);

            if (instruction == default)
            {
                throw new PayInstructionNotFound($"Instruction [{request.InstructionId}] not in specified payroll");
            }

            return _mapper.Map<InstructionDetailsDto>(instruction);
            
        }
    }
}