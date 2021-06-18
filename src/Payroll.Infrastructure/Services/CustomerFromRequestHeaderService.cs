using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Infrastructure.Services
{
    public class CustomerFromRequestHeaderService : ICustomerService
    {
        public const string CustomerApiKey = "X-CustomerId";
            
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CustomerFromRequestHeaderService> _logger;


        public CustomerFromRequestHeaderService(IHttpContextAccessor httpContextAccessor, ILogger<CustomerFromRequestHeaderService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        public Guid GetCustomerId()
        {
            var value = _httpContextAccessor.HttpContext?.Request.Headers[CustomerApiKey].First();
            
            _logger.LogInformation($"Customer-key [{value}]");
            
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(CustomerApiKey, "CustomerId missing in request");
            }

            return Guid.Parse(value.Trim('"'));
        }
    }
}