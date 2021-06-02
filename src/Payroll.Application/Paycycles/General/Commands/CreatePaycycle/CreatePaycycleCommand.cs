using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Payroll.Application.Common.Interfaces;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.General.Commands.CreatePaycycle
{
    public class CreatePaycycleCommand : IRequest<Guid>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Payday { get; set; }
    }
    
    public class CreatePaycycleCommandHandler : IRequestHandler<CreatePaycycleCommand, Guid>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public CreatePaycycleCommandHandler(IPaycyclesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        public async Task<Guid> Handle(CreatePaycycleCommand request, CancellationToken cancellationToken)
        {
            var paycycle = new Paycycle(
                _customerService.GetCustomerId(), 
                request.StartDate,
                request.EndDate,
                request.Payday);

            _context.Paycycles.Add(paycycle);
            await _context.SaveChangesAsync(cancellationToken);

            return paycycle.PaycycleId;
        }
    }
}