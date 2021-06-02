using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.PayInstructions.Commands.Shared;

namespace Payroll.Application.Paycycles.PayInstructions.Commands.UpdatePayInstruction
{
    public class UpdatePayInstructionCommand : IRequest
    {
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
        public Guid PayInstructionId { get; set; }
        
        public Money UnitAmount { get; set; }
        public decimal? Quantity { get; set; }
        public Money TotalAmount { get; set; }
        public string PayCode { get; set; }
        public string Description { get; set; }
    }

    public class UpdatePayInstructionCommandHandler : IRequestHandler<UpdatePayInstructionCommand>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public UpdatePayInstructionCommandHandler(IPaycyclesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdatePayInstructionCommand request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .Include(x => x.Payees)
                .ThenInclude(x=>x.PayInstructions)
                .Where(x => x.CustomerId == _customerService.GetCustomerId())
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

            var instruction = payee.FindInstruction(request.PayInstructionId);

            if (instruction == default)
            {
                throw new PayInstructionNotFound($"Instruction [{request.PayInstructionId}] not in specified payroll");
            }

            // instruction.UnitAmountAmount = request.UnitAmount?.Amount;
            // instruction.UnitAmountCurrency = request.UnitAmount?.Currency;
            instruction.UnitAmount = global::Payroll.Domain.Paycycles.Money.For(request.UnitAmount?.Currency, request.UnitAmount?.Amount);
            instruction.UnitQuantity = request.Quantity;
            // instruction.TotalAmountAmount = request.TotalAmount.Amount;
            // instruction.TotalAmountCurrency = request.TotalAmount.Currency;
            instruction.TotalAmount = global::Payroll.Domain.Paycycles.Money.For(request.TotalAmount.Currency, request.TotalAmount.Amount);
            instruction.PayCode = request.PayCode;
            instruction.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}