using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.General.Commands.ConfirmPaycycle
{
    public class ConfirmPaycycleCommand : IRequest
    {
        public Guid PaycycleId { get; set; }
    }

    public class ConfirmPaycycleCommandHandler : IRequestHandler<ConfirmPaycycleCommand>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public ConfirmPaycycleCommandHandler(IPaycyclesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        public async Task<Unit> Handle(ConfirmPaycycleCommand request, CancellationToken cancellationToken)
        {
            var paycycle = await Queryable
                .Where<Domain.Paycycles.Paycycle>(_context.Paycycles, c => c.CustomerId == _customerService.GetCustomerId())
                .FirstOrDefaultAsync(c => c.PaycycleId == request.PaycycleId, cancellationToken);

            if (paycycle == default)
            {
                throw PaycycleNotFound.ForPaycycleId(request.PaycycleId);
            }
            
            paycycle.Status = PaycycleStatus.AwaitingApproval;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}