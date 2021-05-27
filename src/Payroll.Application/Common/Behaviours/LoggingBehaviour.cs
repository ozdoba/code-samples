using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var customerId = _customerService.GetCustomerId().ToString() ?? string.Empty;

            _logger.LogInformation("Request: {Name} {@CustomerId}{@Request}",
                requestName, customerId, request);
        }
    }
}