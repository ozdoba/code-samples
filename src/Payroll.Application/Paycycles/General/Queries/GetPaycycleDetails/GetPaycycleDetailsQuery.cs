using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.PayInstructions.Queries.Dto;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails
{
    public class GetPaycycleDetailsQuery : IRequest<PaycycleDetailsVm>
    {
        public Guid PaycycleId { get; set; }
    }

    public class GetPaycycleDetailsQueryHandler : IRequestHandler<GetPaycycleDetailsQuery, PaycycleDetailsVm>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public GetPaycycleDetailsQueryHandler(IPaycyclesContext context, ICustomerService customerService, IMapper mapper)
        {
            _context = context;
            _customerService = customerService;
            _mapper = mapper;
        }
        
        public async Task<PaycycleDetailsVm> Handle(GetPaycycleDetailsQuery request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .AsNoTracking()
                .Include(x=>x.Payees)
                .ThenInclude(p=>p.PaymentOptions)
                .Include(x=>x.Payees)
                .ThenInclude(p=>p.PayInstructions)
                .ThenInclude(i=>i.PayCode)
                .Where(c => c.CustomerId == _customerService.GetCustomerId())
                .FirstOrDefaultAsync(c => c.PaycycleId == request.PaycycleId, cancellationToken);

            if (paycycle == default)
            {
                throw PaycycleNotFound.ForPaycycleId(request.PaycycleId);
            }

            var vm = new PaycycleDetailsVm
            {
                Paycycle = _mapper.Map<PaycycleType>(paycycle),
                PayeeCount = paycycle.Payees.Count,
                SettlementDetails = paycycle.Payees.Select(TransformPayee).ToList()
            };

            return vm;
        }

        private SettlementDetailsType TransformPayee(Payee payee)
        {
            SettlementDetailsType transform = new SettlementDetailsType();
            transform.EmployeeNumber = payee.EmployeeNumber;
            transform.SettlementAmount = SummarizePayInstructions(payee.PayInstructions);
            transform.SettlementAccount = _mapper.Map<SettlementAccountType>(payee.PaymentOptions);
            return transform;
        }

        private MoneyType SummarizePayInstructions(IEnumerable<PayInstruction> instructions)
        {
            var payInstructions = instructions.ToList();
            
            var payments = payInstructions
                .Where(i => i.PayCode.Type == PayCodeType.Payment)
                .Select(i => i.TotalAmount).ToList();
    
            Money paymentSummary = default;
            if (payments.Any())
            {
                paymentSummary = payments.Aggregate((x, y) => x + y);
            }
            
            var deductables = payInstructions.Where(i => i.PayCode.Type == PayCodeType.Deductible)
                .Select(i => i.TotalAmount).ToList();
            Money deductableSummary = default;
            if (deductables.Any()) {
                deductableSummary = deductables.Aggregate((x, y) => x + y);
            }

            if (!paymentSummary.Equals(default) && !deductableSummary.Equals(default))
            {
                return _mapper.Map<MoneyType>(paymentSummary + deductableSummary);
            } 
            else if (paymentSummary.Equals(default))
            {
                return _mapper.Map<MoneyType>(deductableSummary);
            }
            else
            {
                return _mapper.Map<MoneyType>(paymentSummary);
            }

            return null;
        }
    }
}