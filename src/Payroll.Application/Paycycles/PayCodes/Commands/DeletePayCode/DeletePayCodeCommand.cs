using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Paycycles.PayCodes.Commands.DeletePayCode
{
    public class DeletePayCodeCommand : IRequest
    {
        public string Code { get; set; }
    }

    public class DeletePayCodeCommandHandler : IRequestHandler<DeletePayCodeCommand>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public DeletePayCodeCommandHandler(IPaycyclesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(DeletePayCodeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PayCodes.FindAsync(_customerService.GetCustomerId(), request.Code);
            
            _context.PayCodes.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}