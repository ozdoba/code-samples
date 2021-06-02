using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Paycycles.Exceptions;

namespace Payroll.Application.Paycycles.PayInstructions.Commands.RemovePayInstruction
{
    public class RemovePayInstructionCommand : IRequest
    {
        public Guid CustomerId { get; set; }
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
        public Guid InstructionId { get; set; }
    }

    public class RemovePayInstructionCommandHandler : IRequestHandler<RemovePayInstructionCommand>
    {
        private readonly IPaycyclesContext _context;
        
        public RemovePayInstructionCommandHandler(IPaycyclesContext context) => _context = context;
        
        public async Task<Unit> Handle(RemovePayInstructionCommand request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .AsTracking()
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

            payee.RemovePayInstruction(instruction);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}