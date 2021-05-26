using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.Exceptions;
using Payroll.Application.Paycycles.Queries.ListPaycycles;

namespace Payroll.Application.Paycycles.Queries.GetPaycycleDetails
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
                .Where(c => c.CustomerId == _customerService.GetCustomerId())
                .FirstOrDefaultAsync(c => c.PaycycleId == request.PaycycleId, cancellationToken);

            if (paycycle == default)
            {
                throw PaycycleNotFound.ForPaycycleId(request.PaycycleId);
            }

            var vm = new PaycycleDetailsVm
            {
                Paycycle = _mapper.Map<PaycycleType>(paycycle)
            };

            return vm;
        }
    }
}