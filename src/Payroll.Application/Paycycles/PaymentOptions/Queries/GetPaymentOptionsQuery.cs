using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.PaymentOptions.Queries.Dto;

namespace Payroll.Application.Paycycles.PaymentOptions.Queries
{
    public class GetPaymentOptionsQuery : IRequest<PaymentOptionsDto>
    {
        public Guid CustomerId { get; set; }
        public Guid PaycycleId { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class GetPaymentOptionsQueryHandler : IRequestHandler<GetPaymentOptionsQuery, PaymentOptionsDto>
    {
        private readonly IPaycyclesContext _context;
        private readonly IMapper _mapper;

        public GetPaymentOptionsQueryHandler(IPaycyclesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaymentOptionsDto> Handle(GetPaymentOptionsQuery request, CancellationToken cancellationToken)
        {
            var paycycle = await _context.Paycycles
                .AsNoTracking()
                .Include(x => x.Payees)
                .ThenInclude(x => x.PaymentOptions)
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
            
            return _mapper.Map<PaymentOptionsDto>(payee.PaymentOptions);
        }
    }
}