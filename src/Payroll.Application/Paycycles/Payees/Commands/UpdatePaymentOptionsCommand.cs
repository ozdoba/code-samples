using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.Payees.Commands
{
    public class UpdatePaymentOptionsCommand : IRequest
    {
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
        
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
            
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BranchCode { get; set; }
        public Address BranchAddress { get; set; }
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
            // var paycycle = await _context.Paycycles
            //     .Include(x => x.Payees)
            //     // .ThenInclude(x=>x.PaymentOptions)
            //     // .ThenInclude(x=>x.BranchAddress)
            //     .FirstOrDefaultAsync(x => x.PaycycleId == request.PaycycleId, cancellationToken);
            //
            // if (paycycle == default)
            // {
            //     throw PaycycleNotFound.ForPaycycleId(request.PaycycleId);
            // }
            //
            // var payee = paycycle.FindPayee(request.EmployeeNumber);
            //
            // if (payee == default)
            // {
            //     throw new Exception($"Employee [{request.EmployeeNumber}] not in specified payroll");
            // }
            //
            // payee.PaymentOptions.AccountHolder = request.AccountHolder;
            // payee.PaymentOptions.AccountNumber = request.AccountNumber;
            // payee.PaymentOptions.BankName = request.BankName;
            // payee.PaymentOptions.SwiftCode = request.SwiftCode;
            // payee.PaymentOptions.BranchCode = request.BranchCode;
            // payee.PaymentOptions.BranchAddress.BuildingNumber = request.BranchAddress?.BuildingNumber;
            // payee.PaymentOptions.BranchAddress.Street = request.BranchAddress?.Street;
            // payee.PaymentOptions.BranchAddress.City = request.BranchAddress?.City;
            // payee.PaymentOptions.BranchAddress.State = request.BranchAddress?.State;
            // payee.PaymentOptions.BranchAddress.PostalCode = request.BranchAddress?.PostalCode;
            // payee.PaymentOptions.BranchAddress.CountryCode = request.BranchAddress?.CountryCode;
            // payee.PaymentOptions.IsoCountryCode = request.IsoCountryCode;
            //
            // await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}