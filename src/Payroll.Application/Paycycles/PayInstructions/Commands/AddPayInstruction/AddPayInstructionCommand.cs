using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Domain.Paycycles;
using Money = Payroll.Application.Paycycles.PayInstructions.Commands.Shared.Money;

namespace Payroll.Application.Paycycles.PayInstructions.Commands.AddPayInstruction
{
    public class AddPayInstructionCommand : IRequest<Guid>
    {
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
        
        public Money UnitAmount { get; set; }
        public decimal? Quantity { get; set; }
        public Money TotalAmount { get; set; }
        public string PayCode { get; set; }
        public string Description { get; set; }
    }

    public class AddPayInstructionCommandHandler : IRequestHandler<AddPayInstructionCommand, Guid>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public AddPayInstructionCommandHandler(IPaycyclesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Guid> Handle(AddPayInstructionCommand command, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .Include(x => x.Payees)
                .ThenInclude(x=>x.PayInstructions)
                .Where(x => x.CustomerId == _customerService.GetCustomerId())
                .FirstOrDefaultAsync(x => x.PaycycleId == command.PaycycleId, cancellationToken);
            
            if (paycycle == default)
            {
                throw new PaycycleNotFound($"Paycycle [{command.PaycycleId}] not found");
            }

            var instruction = new PayInstruction
            {
                InstructionId = Guid.NewGuid(),
                // TotalAmountAmount = command.TotalAmount.Amount,
                // TotalAmountCurrency = command.TotalAmount.Currency,
                TotalAmount = global::Payroll.Domain.Paycycles.Money.For(command.TotalAmount.Currency, command.TotalAmount.Amount),
                PayCode = command.PayCode,
                Description = command.Description,
                UnitAmount = global::Payroll.Domain.Paycycles.Money.For(command.UnitAmount?.Currency, command.UnitAmount?.Amount),
                UnitQuantity = command.Quantity,
            };
            paycycle.AddInstruction(command.EmployeeNumber, instruction);
            
            await _context.SaveChangesAsync(cancellationToken);

            return instruction.InstructionId;
        }
    }
}