using System;
using Microsoft.AspNetCore.Http;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Infrastructure.Services
{
    public class CustomerFromRequestHeaderService : ICustomerService
    {
        public const string CustomerApiKey = "X-CustomerId";
            
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerFromRequestHeaderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public Guid GetCustomerId()
        {
            var value = _httpContextAccessor.HttpContext?.Request.Headers[CustomerApiKey];
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(CustomerApiKey, "CustomerId missing in request");
            }

            return Guid.Parse(value);
        }
    }
}