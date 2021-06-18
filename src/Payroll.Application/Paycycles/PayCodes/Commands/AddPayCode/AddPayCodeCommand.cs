using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Payroll.Application.Common.Interfaces;
using Payroll.Domain.Paycycles;
using PayCodeType = Payroll.Application.Paycycles.PayCodes.Common.PayCodeType;

namespace Payroll.Application.Paycycles.PayCodes.Commands.AddPayCode
{
    public class AddPayCodeCommand : IRequest<string>
    {
        /// <summary>
        /// The unique code of the pay code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// A description of the pay code
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The type of the pay code
        /// </summary>
        public PayCodeType Type { get; set; }
    }

    public class AddPayCodeCommandHandler : IRequestHandler<AddPayCodeCommand, string>
    {
        private readonly IPaycyclesContext _context;
        private readonly ICustomerService _customerService;

        public AddPayCodeCommandHandler(IPaycyclesContext context,ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public Task<string> Handle(AddPayCodeCommand request, CancellationToken cancellationToken)
        {
            var entity = new PayCode
            {
                Code = request.Code,
                Description = request.Description,
                Type = Enum.Parse<Domain.Paycycles.PayCodeType>(request.Type.ToString()),
                CustomerId = _customerService.GetCustomerId()
            };

            _context.PayCodes.Add(entity);

            _context.SaveChangesAsync(cancellationToken);
            
            return Task.FromResult(entity.Code);
        }
    }
}