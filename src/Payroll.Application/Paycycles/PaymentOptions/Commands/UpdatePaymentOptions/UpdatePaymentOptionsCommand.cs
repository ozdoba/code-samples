using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.PaymentOptions.Commands.Shared;

namespace Payroll.Application.Paycycles.PaymentOptions.Commands.UpdatePaymentOptions
{
    public class UpdatePaymentOptionsCommand : IRequest
    {
        public Guid CustomerId { get; set; }
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
        
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
            
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BranchCode { get; set; }
        public BankAddressType BranchAddress { get; set; }
        public string IsoCountryCode { get; set; }
    }

    public class UpdatePaymentOptionsCommandHandler : IRequestHandler<UpdatePaymentOptionsCommand>
    {
        private readonly IPaycyclesContext _context;

        public UpdatePaymentOptionsCommandHandler(IPaycyclesContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdatePaymentOptionsCommand request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .Include(x => x.Payees)
                .ThenInclude(x=>x.PaymentOptions)
                .ThenInclude(x=>x.BranchAddress)
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

            payee.UpdatePaymentOptions(
                request.AccountHolder,
                request.AccountNumber,
                request.BankName,
                request.SwiftCode,
                request.BranchCode,
                request.BranchAddress?.BuildingNumber,
                request.BranchAddress?.Street,
                request.BranchAddress?.City,
                request.BranchAddress?.State,
                request.BranchAddress?.PostalCode,
                request.BranchAddress?.CountryCode,
                request.IsoCountryCode);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}