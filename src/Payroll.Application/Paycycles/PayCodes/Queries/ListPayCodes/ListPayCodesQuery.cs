using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles.PayCodes.Common;

namespace Payroll.Application.Paycycles.PayCodes.Queries.ListPayCodes
{
    public class ListPayCodesQuery : IRequest<ListPayCodesResponse>
    {
    }

    public class ListPayCodesQueryHandler : IRequestHandler<ListPayCodesQuery, ListPayCodesResponse>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public ListPayCodesQueryHandler(IPaycyclesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<ListPayCodesResponse> Handle(ListPayCodesQuery request, CancellationToken cancellationToken)
        {
            var customerId = _customerService.GetCustomerId();
            
            var payCodes = await _context.PayCodes
                .Where(x => x.CustomerId == customerId)
                .ToListAsync(cancellationToken);
            
            return new ListPayCodesResponse
            {
                CustomerId = customerId.ToString(),
                Data = payCodes.Select(e => new PayCodeDetails
                {
                    Code = e.Code,
                    Description = e.Description,
                    Type = Enum.Parse<PayCodeType>(e.Type.ToString())
                }).ToList()
            };
        }
    }
}